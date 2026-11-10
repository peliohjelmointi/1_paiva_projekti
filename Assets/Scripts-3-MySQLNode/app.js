const express = require('express') //express on noden suosituin moduli serverin käyttämiseen
const mysql = require('mysql2')  //tätä ei vielä 1.testissä käytössä, modulilla saadaan mysql-yhteys

const app = express() //luodaan express-applikaatio kutsumalla express-modulin express-funktiota
const port = 3000 //aseta jokin vapaana oleva portti jos ei tällä toimi 

//route endpoints
app.get('/', (req, res) => { //tämä on reitti osoitteeseen  localhost:3000/
    console.log("HELLO WORLD!")
    res.send("HELLO HELLO") 
    //ilman res.sendiä web-sivu ei koskaan lähetä responsea
})

app.get('/test', (req,res)=> { //tämä on reitti osoitteeseen localhost:3000/test
    res.send("unityssä painettin välilyöntiä")
    //res.redirect('http://www.google.com')
})

app.get('/unitytest', (req, res) =>{
    console.log("Space was pressed in Unity")

    var dummyData = {
        "action": "InstantiateCube",
        "position": {
            "x": 3,
            "y": 0,
            "z": 0
        }        
    }
    res.json(dummyData)
})

app.listen(port, () => { //asetetaan ohjelma kuuntelemaan porttia 3000. Tässä tulisi olla myös virheenkäsittely; tässä vain minimaalinen esimerkki
    console.log("Server is listening")
})