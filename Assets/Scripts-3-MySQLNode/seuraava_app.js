var express = require('express')
var mysql = require('mysql2')
var app = express()
var port = process.env.port || 3000

app.get('/unitytest', (req, res) => {
    console.log("Space was pressed in Unity")
    var dummyData = {
        "action": "InstantiatePlayer",
        "position": {
            "x": 3,
            "y": 0,
            "z": 0
        }
    }
    res.json(dummyData)
})
app.get('/api/player/:id', (req, res) => {
    console.log("Haetaan tietokannasta pelaaja, jonka id on:" + req.params.id)
    var conn = mysql.createConnection({
        host: "localhost",
        user: "root",
        password: "",
        database: "unitydb"
    })
    conn.connect()
    //#region preventing sql_injection
    const id = parseInt(req.params.id, 10);
    if (Number.isNaN(id)) {
        return res.status(400).send("Invalid id");
    }
    // dangerous way: (injection possible)
    //conn.query("SELECT nickname FROM players WHERE id =" + req.params.id) // user can enter: 1 OR 1=1
    //better way using placeholders:
    conn.query(
        "SELECT id, nickname, firstname, lastname FROM players WHERE id = ?",[id],
        (err, results) => {
            if (err) {
                return res.status(500).send("Database error");
            }
            console.log(results[0].nickname)
            console.log(results[0].firstname)
            console.log(results[0].lastname)
            
            //res.json will also be shown on webpage without unity
            //res.json(results); //all info, including id
            res.json({nickname: results[0].nickname, firstname: results[0].firstname, lastname:results[0].lastname})
        }
    )
    conn.end()
    //#endregion
})
app.listen(port, () => {
    console.log("Server is listening")
})