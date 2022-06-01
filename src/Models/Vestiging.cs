using System.Collections.Generic;

public class Vestiging{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Adress{get;set;}
    public string Plaats{get;set;}
    public List<Hulpverlener> Hulpverleners{get;set;}
}