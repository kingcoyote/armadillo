{
    shortsword : {
        name : "Shortsword",
        itemType : ["sword"],
        statBoosts :
        [
           { stat : "attack", amount : "10" }
        ],
        ability : "lunge",
        cost : 10,
        targetGrid : "target_melee_small"
    },
    longsword : {
        name: "Longsword",
        itemType : ["sword"],
        statBoosts :
        [
            { stat : "attack", amount : "20" }
        ],
        ability : "cleave",
        cost : 25,
        targetGrid : "target_melee_small"
    },
    greatsword : {
        name: "Greatsword",
        itemType: ["sword"],
        statBoosts:
        [
            { stat : "attack", amount : "30" },
            { stat : "hit", amount : "10" }
        ],
        cost : 100,
        targetGrid : "target_melee_small"
    }
}