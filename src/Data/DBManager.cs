using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DBManager : IdentityDbContext<ApplicationUser>
{
    public DBManager(DbContextOptions<DBManager> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // any unique string id
        const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
        const string ADMIN_ROLE_ID = "ad376a8f-9eab-4bb9-9fca-30b01540f445";

        const string ADMIN_ID2 = "a18be9c0-aa65-4af8-bd17-00bd9344e574";
        const string ADMIN_ROLE_ID2 = "ad376a8f-9eab-4bb9-9fca-30b01540f445";

        const string MOD_ID = "a99be1c0-aq65-4af8-br17-00bd9674e571";

        const string clientRole = "e8431a45-405f-4932-8819-46b179f1d3d3";
        const string hulpverlenerRole = "21879a36-cc2c-4fb9-9213-2708b962b92e";
        const string ouderRole = "3e0f2af8-69ce-4d39-ae6a-440e388b3ca3";
        const string moderatorRole = "e7c5f138-26ff-4413-a2e2-273a11c334d4";

        const string hulpverlenerid1 = "6c26c683-3d56-4b10-a671-9c0565acba97";
        const string hulpverlenerid2 = "4a02ed90-00c0-4a74-b95e-a54722f1c734";
        const string hulpverlenerid3 = "8c74234a-facb-40fb-b812-f45b1f328867";
        const string hulpverlenerid4 = "e0fd4978-7f42-42d9-a3d8-0efbdd40188b";

        builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Client", NormalizedName = "CLIENT", Id = clientRole, ConcurrencyStamp = Guid.NewGuid().ToString() });
        builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = ADMIN_ROLE_ID, ConcurrencyStamp = Guid.NewGuid().ToString() });
        builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Hulpverlener", NormalizedName = "HULPVERLENER", Id = hulpverlenerRole, ConcurrencyStamp = Guid.NewGuid().ToString() });
        builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Ouder", NormalizedName = "OUDER", Id = ouderRole, ConcurrencyStamp = Guid.NewGuid().ToString() });
        builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Moderator", NormalizedName = "MODERATOR", Id = moderatorRole, ConcurrencyStamp = Guid.NewGuid().ToString() });


        var hasher = new PasswordHasher<ApplicationUser>();
        builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = ADMIN_ID,
            UserName = "admin@admin.com",
            NormalizedUserName = "ADMIN@ADMIN.COM",
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            EmailConfirmed = false,
            LockoutEnabled = true,
            PasswordHash = hasher.HashPassword(null, "Test123!"),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        });

        builder.Entity<Vestiging>().HasData(new Vestiging
        {
            Id = 1,
            Name = "Vesteging Den Haag",
            Adress = "Melis Stokelaan 156",
            Plaats = "Den Haag"

        });
        builder.Entity<Hulpverlener>().HasData(new Hulpverlener
        {
            Id = 1,
            Name = "Kees",
            Adres = "",
            Specialisatie = "Eetstoornissen",
            Intro = "Ik heet Kees Closed, geboren in 1978 in Monster en de jongste van twee kinderen. Mijn ouders zijn gescheiden toen ik zes was en ik ben opgegroeid in het Westland. Ik ben heel nieuwsgierig naar het afwijkend eetgedrag van jongeren, waarom ze doen wat ze doen en wat hun verhaal is. Ik heb vroeger zelf last gehad van een eetstoornis en ben hier door middel van de juiste hulp vanaf gekomen. Dit heeft mij nieuwsgierig gemaakt naar hoe anderen dat ervaren en hoe ik hun daarmee kan helpen. ",
            Study = "Ik heb orthopedagogiek gestudeerd aan de Universiteit van Leiden. Bij mijn afstuderen heb ik mij gespecialiseerd in de behandeling van eetstoornissen.  ",
            OverJou = "Heb jij en vermoede dat je een eetstoornis hebt? Het grootste deel van de mensen heeft een verstoord lichaamsbeeld. Dit zorgt ervoor dat je bang bent om in gewicht aan te komen. Deze mensen letten vaak op wat ze eten, sporten overmatig of braken zelfs. Het gewicht wordt dan een obsessie en het lijnen, sporten of braken wordt een verslaving. ",
            Behandeling = "Eerst moet er vastgesteld worden of je een eetstoornis hebt, en zo ja wat voor eetstoornis je hebt. Daarna kijken we hoe we jou kunnen helpen. Een eetstoornis is namelijk een psychische aandoening die niet vanzelf over gaat. We kunnen de eetstoornis het best behandelen met Cognitieve gedragstherapie. Cognitieve gedragstherapie is de meest onderzochte en effectieve psychologische behandeling bij eetstoornissen. In de behandeling wordt onder andere onderzocht welke gedachten er zijn over eten, gewicht en lichaamsvormen. Daarnaast wordt er ook onderzocht of deze gedachten kloppen. ",
            Foto = "Screenshot_57.png",
            VestigingId = 1
        },
        new Hulpverlener
        {
            Id = 2,
            Name = "Krystel",
            Adres = "",
            Specialisatie = "Dyslectie",
            Intro = "Mijn naam is Krystel Vegter, geboren in 1990 in Honselersdijk en ik heb een broer. Mijn ouders zijn gescheiden toen ik zes was en ik ben opgegroeid in het Westland bij mijn moeder. Mijn hobby’s zijn schilderen, koken en een goed boek lezen. Door het scheiden van mijn ouders ben ik al op jongen leeftijd geïnteresseerd geraakt in beweegredenen van mensen en specifiek in jonge mensen die nog volop in ontwikkeling zijn. Door mijn nieuwsgierigheid te combineren met mijn interesse heb ik mijn perfecte beroep gevonden. ",
            Study = "Ik heb gestudeerd aan de universiteit van leiden, waar ik mijn bachelor in psychologie heb gehaald en een master gespecialiseerd rondom problemen bij jongeren. ",
            OverJou = "Er bestaat een vermoeden dat jij dyslectie hebt. Het is je opgevallen dat lezen toch langzamer gaat dan de rest van de kinderen/jongeren om je heen. Daarnaast zijn alle taalvakken moeilijk te volgen.   Waardoor ook schrijven je niet altijd even makkelijk afgaat. Grote kans dat een van je leraren dit is opgevallen en dit heeft aangegeven bij ons. Wees maar gerust want er is nog niks zeker en al zou je het wel hebben. dyslectie hoeft je schoolcarrière zeker niet in de weg te gaan zitten, want er zijn genoeg hulpmiddelen. ",
            Behandeling = "Eerst gaan wij is een goed gesprek hebben over wie jij bent en wat je doet, ik ben namelijk heel benieuwd naar je. Daarna zal ik wat tests samen met jou afnemen. Deze test bestaan vooral Uit lees- en schrijfoefeningen. Waaruit we zullen zien of je wel daadwerkelijk dyslectie hebt. Zo ja, dan zullen we meer afspraken maken. Tijdens deze afspraken ga ik jou helpen met lezen en schrijven. We gaan samen werken aan trucjes die bij jou aansluiten om je zo goed mogelijk te helpen om op het juiste niveau te komen. ",
            Foto = "Screenshot_58.png",
            VestigingId = 1
        },
        new Hulpverlener
        {
            Id = 3,
            Name = "Sugodies Allen",
            Adres = "",
            Specialisatie = "Angststoornis",
            Intro = "Mijn naam is Sugodies Allen, geboren in 1978 en opgegroeid in Den Haag, Ik ben de jongste in een gezin van zes kinderen. Mijn hobby’s zijn: lezen, schrijven en mensen helpen waar ik geschikt voor ben. Van jongs af aan wilde ik altijd iets doen om mensen met hun angst te helpen en overkomen. Hierdoor streef ik om een orthopedagoog te worden gespecialiseerd in cliënten helpen die te maken hebben met een angststoornis. ",
            Study = "Nadat ik klaar was met het VWO heb ik orthopedagogiek gestudeerd aan de Universiteit van Amsterdam. Bij het afstuderen heb ik mij gespecialiseerd in de behandelingen van angststoornissen. ",
            OverJou = "Angststoornis is dat je bang bent zelfs als de situatie geen gevaar geeft. Je raakt hierdoor in paniek en ook krijgen van serieuze klachten. Kenmerken van angststoornis die vaak gebeuren zijn: hoofdpijn, buikpijn, slaapproblemen en prikkelbaarheid. Maar ook bijzondere kenmerken: ademnood, hartkloppingen, tinteling of een doof gevoel in jouw ledematen. Er zijn nog vele andere kenmerken die je nog tegenaan loopt bij angststoornis. ",
            Behandeling = "Wat we eerst doen is nagaan wat voor soort angststoornis jij te maken mee heeft. Dit doen we met een aantal testen. Jouw ouders of verzorgers mogen bij de testen zijn. Als het duidelijk is wat voor soort angststoornis het is gaan wij een behandelplan hiervoor samenstellen.   Eén van de eerste stappen is een gesprek voeren over gebeurtenissen in jouw leven. Dat is puur om uit te vogelen wat de angststoornis heeft veroorzaakt. Als we een oorzaak hebben gevonden kunnen progressieve gesprekken houden zodat jij in jouw vel zit. Je kan mij bellen of mailen voor meer informatie of vragen. We plannen gesprekken in om te zien hoe wij verder gaan erna. Ik help jouw heel graag en je bent van alle harte welkom. ",
            Foto = "Screenshot_59.png",
            VestigingId = 1
        },
        new Hulpverlener
        {
            Id = 4,
            Name = "Melissa Wiedegem",
            Adres = "",
            Specialisatie = "ADHD",
            Intro = "Mijn naam is Melissa Wiedegem, geboren in 1995 in Rotterdam met een tweelingzus. Ik merkte op jonge leeftijd al dat ik een passie begon op te bouwen voor het helpen van anderen. Dit groeide later uit tot het helpen van kinderen met ADHD. ",
            Study = "Ik heb eerder al verschillende rollen vervuld zoals leerkracht en speciale leerkracht. In 2017 ben ik afgestudeerd als orthopedagoog aan de Universiteit van Utrecht. Ik heb mij bij het afstuderen gespecialiseerd in de behandeling van ADHD. ",
            OverJou = "Bij jou bestaat het vermoeden dat je ADHD hebt. Als je dit hebt, werken je hersenen net iets anders dan bij iemand zonder ADHD. De meeste mensen hebben een soort filter in hun hersenen. Die zorgt ervoor dat niet alle prikkels (zoals geluiden of dingen in de omgeving) even hard binnen komen. Dit is bij ADHD anders. Bij ADHD komen er juist te veel prikkels binnen doordat er minder goed wordt gefilterd. Daardoor heb jij misschien moeite met het focussen op de belangrijke dingen van dat moment zonder afgeleid te worden. Zo zijn er nog meer dingen waar jij tegen aan kan lopen die bij ADHD horen. ",
            Behandeling = "We gaan gezamenlijk uitzoeken of jij ADHD hebt. Dit doen we door middel van een paar testen. Je ouder(s)/verzorger(s) mogen hier ook bij zijn als jij dat prettig vindt. De testen doen we omdat ik namelijk erg benieuwd ben naar jou en hoe jij ADHD ervaart. Op basis van deze testen kan ik oefeningen met je doen en tips geven om uiteindelijk beter met ADHD om te gaan.   Je bent zeer welkom en ik wil je graag helpen, als je nog meer informatie wilt kan je me gerust emaillen of bellen. ",
            Foto = "Screenshot_60.png",
            VestigingId = 1
        });


        builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = ADMIN_ID2,
            UserName = "admin2@admin.com",
            NormalizedUserName = "ADMIN2@ADMIN.COM",
            Email = "admin2@admin.com",
            NormalizedEmail = "ADMIN2@ADMIN.COM",
            EmailConfirmed = false,
            LockoutEnabled = true,
            PasswordHash = hasher.HashPassword(null, "Test123!"),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }, new ApplicationUser
        {
            Id = MOD_ID,
            UserName = "mod@mod.com",
            NormalizedUserName = "MOD@MOD.COM",
            Email = "mod@mod.com",
            NormalizedEmail = "MOD@MOD.COM",
            EmailConfirmed = false,
            LockoutEnabled = true,
            PasswordHash = hasher.HashPassword(null, "Test123!"),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }, new ApplicationUser
        {
            Id = hulpverlenerid1,
            HulpverlenerId = 1,
            UserName = "kees@gmail.com",
            NormalizedUserName = "KEES@GMAIL.COM",
            Email = "kees@gmail.com",
            NormalizedEmail = "KEES@GMAIL.COM",
            EmailConfirmed = false,
            LockoutEnabled = true,
            PasswordHash = hasher.HashPassword(null, "Kees123@"),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }, new ApplicationUser
        {
            Id = hulpverlenerid2,
            HulpverlenerId = 2,
            UserName = "krystel@gmail.com",
            NormalizedUserName = "KRYSTEL@GMAIL.COM",
            Email = "krystel@gmail.com",
            NormalizedEmail = "KRYSTEL@GMAIL.COM",
            EmailConfirmed = false,
            LockoutEnabled = true,
            PasswordHash = hasher.HashPassword(null, "Krystel123@"),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }, new ApplicationUser
        {
            Id = hulpverlenerid3,
            HulpverlenerId = 3,
            UserName = "sugodies@gmail.com",
            NormalizedUserName = "SUGODIES@GMAIL.COM",
            Email = "sugodies@gmail.com",
            NormalizedEmail = "SUGODIES@GMAIL.COM",
            EmailConfirmed = false,
            LockoutEnabled = true,
            PasswordHash = hasher.HashPassword(null, "Sugodies123@"),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }, new ApplicationUser
        {
            Id = hulpverlenerid4,
            HulpverlenerId = 4,
            UserName = "melissa@gmail.com",
            NormalizedUserName = "MELISSA@GMAIL.COM",
            Email = "melissa@gmail.com",
            NormalizedEmail = "MELISSA@GMAIL.COM",
            EmailConfirmed = false,
            LockoutEnabled = true,
            PasswordHash = hasher.HashPassword(null, "Melissa123@"),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        });
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = ADMIN_ROLE_ID,
            UserId = ADMIN_ID
        }, new IdentityUserRole<string>
        {
            RoleId = ADMIN_ROLE_ID2,
            UserId = ADMIN_ID2
        }, new IdentityUserRole<string>
        {
            RoleId = hulpverlenerRole,
            UserId = hulpverlenerid1
        }, new IdentityUserRole<string>
        {
            RoleId = hulpverlenerRole,
            UserId = hulpverlenerid2
        }, new IdentityUserRole<string>
        {
            RoleId = hulpverlenerRole,
            UserId = hulpverlenerid3
        }, new IdentityUserRole<string>
        {
            RoleId = hulpverlenerRole,
            UserId = hulpverlenerid4
        });

        builder.Entity<ApplicationUserChat>().HasKey(ac => new { ac.ApplicationUserId, ac.ChatId });

        builder.Entity<ApplicationUserChat>()
            .HasOne<ApplicationUser>(ac => ac.ApplicationUser)
            .WithMany(s => s.ApplicationUserChats)
            .HasForeignKey(ac => ac.ApplicationUserId);


        builder.Entity<ApplicationUserChat>()
            .HasOne<Chat>(ac => ac.Chat)
            .WithMany(s => s.ApplicationUserChats)
            .HasForeignKey(ac => ac.ChatId);

        builder.Entity<Report>()
            .HasOne<Message>(r => r.Message)
            .WithMany(m => m.Reports)
            .HasForeignKey(r => r.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Report>()
            .HasOne<ApplicationUser>(r => r.Handler)
            .WithMany(au => au.HandledReports)
            .HasForeignKey(r => r.HandlerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Chat>()
            .HasOne<SelfHelpGroup>(c => c.SelfHelpGroup)
            .WithOne(s => s.Chat)
            .HasForeignKey<SelfHelpGroup>(s => s.ChatId);

        builder.Entity<ChatFrequency>()
            .HasOne<Ouder>(cf => cf.Ouder)
            .WithMany(o => o.ChatFrequencies)
            .HasForeignKey(cf => cf.OuderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ChatFrequency>()
            .HasOne<Hulpverlener>(cf => cf.Hulpverlener)
            .WithMany(h => h.ChatFrequencies)
            .HasForeignKey(cf => cf.HulpverlenerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ChatFrequency>()
            .HasOne<Chat>(cf => cf.Chat)
            .WithOne(h => h.ChatFrequency)
            .HasForeignKey<Chat>(h => h.ChatFrequencyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Melding>()
            .HasOne<Hulpverlener>(m => m.Hulpverlener)
            .WithMany(h => h.Meldingen)
            .HasForeignKey(m => m.HulpverlenerId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ApplicationUserChat> ApplicationUserChats { get; set; }
    public DbSet<Client> Clienten { get; set; }
    public DbSet<Ouder> Ouders { get; set; }
    public DbSet<Hulpverlener> Hulpverleners { get; set; }
    public DbSet<Aanmelding> Aanmeldingen { get; set; }
    public DbSet<SelfHelpGroup> SelfHelpGroups { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Moderator> Moderator { get; set; }
    public DbSet<Vestiging> Vestigingen { get; set; }
    public DbSet<ChatFrequency> ChatFrequencies { get; set; }
    public DbSet<Melding> Meldingen { get; set; }
}