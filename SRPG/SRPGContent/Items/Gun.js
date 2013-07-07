{
    pistol : {
        name : "Pistol",
        itemType : ["gun"],
        statBoosts :
        [
            { stat : "hit", amount : "10" }
        ],
        ability : "headshot",
        cost : 10,
        targetGrid : "target_gun_small"
    },
    revolver : {
        name: "Revolver",
        itemType : ["gun"],
        statBoosts :
        [
            { stat : "hit", amount : "20" }
        ],
        ability : "drill",
        cost : 25,
        targetGrid : "target_gun_medium"
    },
    bigiron : {
        name: "Big Iron",
        itemType: ["gun"],
        statBoosts:
        [
            { stat : "hit", amount : "30" },
            { stat : "speed", amount : "10" }
        ],
        cost : 100,
        targetGrid : "target_gun_large"
    }
}