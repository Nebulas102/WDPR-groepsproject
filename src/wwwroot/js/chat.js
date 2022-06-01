const messageForm = document.getElementById("chatMessageForm");
const messageBox = document.getElementById("chatMessageInput");
const messages = document.getElementById("chatMessageBox");
const displayChatInput = document.getElementById("displayChatInput");

const showGroups = document.getElementById("ShowGroups");
const showChats = document.getElementById("ShowChats");

const reportForm = document.getElementById("reportForm");
const reportInput = document.getElementById("reportInput");
const reportModal = document.getElementById("reportModal");

const chatsOrGroupsDisplay = document.getElementById("chatsOrGroupsDisplay");

const authorId = document.getElementById("authorId");
const chatId = document.getElementById("chatId");
const chatName = document.getElementById("chatName");
const authorUsername = document.getElementById("authorUsername");
const determineSendBy = document.getElementById("determineSendBy");

const messagesApiUri = "/api/Message";
const chatHandlerApiUri = "/api/ChatHandler";
const timeOptions = { year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric', second: '2-digit' };

//signalR connection om te invoken
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/Chat/ChatApi/")
    .configureLogging(signalR.LogLevel.Information)
    .build();

//kiezen dat de er alleen chats of zelfhulpgroepen moet laten zien bij sidebar    
let decideDisplayChatsOrGroups = "";

//de modal te closen van de report    
function closeModal() {
    reportModal.style.display = "none";
    document.getElementsByClassName("modal-backdrop")[0].style.display = "none";
    reportModal.classList.remove("show");
}

//start signalR connection
connection.start()
    .then(() => console.log('connected!'))
    .then(getChatViewModel())
    .then(displayChatInput.style.display = "none")
    .catch(console.error);

function InitializeReport() {
    var reportClasses = document.getElementsByClassName("chatMessageReport");
    for(var i = 0; i < reportClasses.length; i++) {
        reportClasses[i].addEventListener("click", (evt) => {
            var messageId = document.getElementById("reportMessageId");
            messageId.value = evt.target.getAttribute("data-message-id");

            reportForm.addEventListener("submit", (ev) => {
                ev.preventDefault();
                ev.stopImmediatePropagation();
            
                const item = {
                    content: reportInput.value.trim(),
                    isHandled: false,
                    messageId: messageId.value,
                }
            
                fetch(`${chatHandlerApiUri}/PostReport`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(item)
                })
                    .then(response => response.json())
                    .then(() => {
                        reportInput.value = "";
                    })
                    .catch(error => console.error("Unable to add report.", error));
             
                closeModal();
            });
        });
    }
}

//krijg de chatviewmodel met basis op current user
//returns: alle data van tussentabel applicationuserchat, alle data chat, alle data zelfhulpgroepen
function getChatViewModel() {
    fetch(`${chatHandlerApiUri}/getChatViewModel`, {
        headers: {
            'Content-Type': 'application/json',
            'Cache-Control': 'no-cache, no-store, must-revalidate'
        }
    })
        .then(response => response.json())
        .then(data => displayChatsOrGroups(data))
        .catch(error => console.error('Unable to retrieve chatviewmodel.', error));
}

//krijg de chatviewmodel met basis op current user van de current chat/zelfhulpgroep
//returns: current data van tussentabel applicationuserchat, current data chat, current data zelfhulpgroepen
function getChatHandlerViewModel() {

    fetch(`${chatHandlerApiUri}/getChatHandlerViewModel?chatid=${chatId.value}`, {
        headers: {
            'Content-Type': 'application/json',
            'Cache-Control': 'no-cache, no-store, must-revalidate'
        }
    })
        .then(response => response.json())
        .then(data => updateChatValues(data))
        .catch(error => console.error('Unable to retrieve chathandlerviewmodel.', error));
}

//krijg alle berichten returned van current chat
function getMessages() {
    fetch(`${messagesApiUri}/chat/${chatId.value}`, {
        headers: {
            'Content-Type': 'application/json',
            'Cache-Control': 'no-cache, no-store, must-revalidate'
        }
    })
        .then(response => response.json())
        .then(data => displayMessagesInChat(data))
        .catch(error => console.error('Unable to retrieve messages.', error));
}

//update de values van currentChat id en naam als er naar een andere chat/zelfhulpgroep wordt verandert
//voeg de gebruiker clientside toe aan de nieuwe chat
function updateChatValues(data) {
    chatId.value = data.currentChat.id;
    chatName.value = data.currentChat.name;

    connection.invoke("AddToChat", chatName.value);
}

//UTF-16 function call van de ChatHandler om realtime een bericht te versturen en te posten
connection.on('newMessage', (sender, messageText) => {

    const item = {
        content: messageText,
        created: new Date().toISOString(),
        authorid: authorId.value,
        chatid: chatId.value
    };

    var obj = {
        item,
        determineSendBy: determineSendBy.value == "true" ? true : false
    }

    determineSendBy.value = "false";
    messageBox.value = "";

    fetch(messagesApiUri, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Cache-Control': 'no-cache, no-store, must-revalidate'
        },
        body: JSON.stringify(item)
    })
    .then(data => data.json())
    .then((data) => {
        obj.item = data;
        JSON.stringify(obj);
        chatMessageDisplaySignalR(obj, sender, messageText);
        InitializeReport();
    });
});

//wanneer submit wordt geklikt van de messageform wordt het bericht verstuurt naar de signalR client
messageForm.addEventListener('submit', ev => {
    ev.preventDefault();
    const message = messageBox.value;
    determineSendBy.value = "true";
    connection.invoke('SendMessage', authorUsername.value, message, chatName.value).catch(function (err) {
        messageBox.value = "";
        return console.error(err.toString());
    });
});

//sluit de connectie van een chat clientside wanneer een chat wordt geklikt
//sinds je een andere chat gaat joinen moet de vorige chat clientside verwijdert worden
function chatClickEvent(id, name, item) {
    connection.onclose(connection.invoke("RemoveFromChat", chatName.value));

    chatId.value = id;
    chatName.value = name;

    for (let i = 0; i < chatsOrGroupsDisplay.children.length; i++) {
        if (chatsOrGroupsDisplay.children[i].classList.contains("activehoverrood")) {
            chatsOrGroupsDisplay.children[i].classList.remove("activehoverrood");
        }
    }

    displayChatInput.style.display = "flex"

    messages.innerHTML = '';

    getChatHandlerViewModel();
    getMessages();

    messages.scrollTop = messages.scrollHeight;

    item.classList.add("activehoverrood");
}

function changeDecideDisplayChatsOrGroupsToChat() {
    decideDisplayChatsOrGroups = "chats";
    showGroups.classList.remove("active-decideDisplayChatsOrGroups");
    showChats.classList.add("active-decideDisplayChatsOrGroups");
    chatsOrGroupsDisplay.innerHTML = "";
    getChatViewModel();
}

function changeDecideDisplayChatsOrGroupsToGroup() {
    decideDisplayChatsOrGroups = "selhelpgroups";
    showChats.classList.remove("active-decideDisplayChatsOrGroups");
    showGroups.classList.add("active-decideDisplayChatsOrGroups");
    chatsOrGroupsDisplay.innerHTML = "";
    getChatViewModel();
}


//laat de chats of zelfhulpgroepen zien in de sidebar
function displayChatsOrGroups(data) {
    if(decideDisplayChatsOrGroups == "chats") {
        data.chats.forEach(item => {
            if(item.selfHelpGroup == null) {
                itemDisplayChatsOrGroups(item);
            }
        });
    } else {
        data.chats.forEach(item => {
            if(item.selfHelpGroup != null) {
                itemDisplayChatsOrGroups(item);
            }
        });
    }
}

//laat de oude berichten zien van de current chat
function displayMessagesInChat(data) {

    data.forEach(item => {
        chatMessageDisplay(item);
    });
    InitializeReport();
}

//een view voor om de chats of zelfhulpgroepen te laten zien in de sidebar
function itemDisplayChatsOrGroups(data) {
    var item = document.createElement("div");
    item.className = "list-group-item list-group-item-action text-white rounded-0";
    item.setAttribute("onclick", `chatClickEvent(${data.id} ,"${data.name}", this)`);

    item.innerHTML =
        '<div class="media">' +
        '<div class="media-body ml-4">' +
        '<div class="d-flex align-items-center justify-content-between mb-1">' +
        `<h6 class="mb-0 text-primary">${data.name}</h6><small class="text-primary small font-weight-bold">Date</small>` +
        '</div>' +
        '<p class="font-italic mb-0 text-small text-primary">Description</p>' +
        '</div>' +
        '</div>';

    chatsOrGroupsDisplay.append(item);
}

//een view om chat bericht zien basis op signalR data
function chatMessageDisplaySignalR(data, sender, messageText) {
    if (data.determineSendBy == false) {
        messages.innerHTML += '<div class="media w-50 mb-3 chat-message">' +
            '<div class="media-body ml-3">' +
            '<div class="bg-light rounded py-2 px-3 mb-2">' +
            `<p class="text-small mb-0 text-muted chat-text author-text">${messageText}</p>` +
            '</div>' +
            `<p class="small text-muted created-datetime">${new Date(data.item.created).toLocaleDateString("nl-NL", timeOptions)}</p>` +
            `<i class="small text-muted">${sender}</i><span data-toggle="modal" data-target="#reportModal" data-message-id="${data.item.id}" class="chatMessageReport">Report</span>` +
            '</div>' +
            '</div >';
    } else {
        messages.innerHTML += '<div class="media ml-auto w-50 mb-3 chat-message">' +
            '<div class="media-body ml-3">' +
            '<div class="bg-primary rounded py-2 px-3 mb-2 chat-text participant-text">' +
            `<p class="text-small mb-0 text-white chat-text author-text">${messageText}</p>` +
            '</div>' +
            `<p class="small text-muted created-datetime">${new Date(data.item.created).toLocaleDateString("nl-NL", timeOptions)}</p>` +
            `<i class="small text-muted">${sender}</i>` +
            '</div>' +
            '</div>' +
            '</div >';
    }
}

//een view om chat bericht te laten zien basis op simpel message object
function chatMessageDisplay(data) {
    if (data.authorId != authorId.value) {
        messages.innerHTML += '<div class="media w-50 mb-3 chat-message">' +
            '<div class="media-body ml-3">' +
            '<div class="bg-light rounded py-2 px-3 mb-2">' +
            `<p class="text-small mb-0 text-muted chat-text author-text">${data.content.replace(/[\<]/gi, "&lt;")}</p>` +
            '</div>' +
            `<p class="small text-muted created-datetime">${new Date(data.created).toLocaleDateString("nl-NL", timeOptions)}</p>` +
            `<i class="small text-muted">${data.author.userName}</i><span data-toggle="modal" data-target="#reportModal" data-message-id="${data.id}" class="chatMessageReport">Report</span>` +
            '</div>' +
            '</div >';
    } else {
        messages.innerHTML += '<div class="media ml-auto w-50 mb-3 chat-message">' +
            '<div class="media-body ml-3">' +
            '<div class="bg-primary rounded py-2 px-3 mb-2 chat-text participant-text">' +
            `<p class="text-small mb-0 text-white chat-text author-text">${data.content.replace(/[\<]/gi, "&lt;")}</p>` +
            '</div>' +
            `<p class="small text-muted created-datetime">${new Date(data.created).toLocaleDateString("nl-NL", timeOptions)}</p>` +
            `<i class="small text-muted">${data.author.userName}</i>` +
            '</div>' +
            '</div>' +
            '</div >';
    }
}