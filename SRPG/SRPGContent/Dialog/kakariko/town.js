{
    statue : {
        nodes: [
            {
                identifier: 1,
                text: "It's a statue with a weather vein at the top.",
                options: [
                    { text: "Default", node: 2 }
                ]
            },
            {
                identifier: 2,
                text: "If there was wind, it might be more useful. This is not a hint for a future puzzle in the game."
            }
        ]
    },
    mailbox : {
        nodes: [
            {
                identifier: 1,
                text: "There is nothing in the mailbox."
            }
        ]
    },
    cliff : {
        nodes : [
            {
                identifier:1,
                text : "Looks unsafe to jump. Want to do it anyway?",
                options: [
                    { text: "Sure!", node: 2 },
                    { text: "Nope!", node: 4 }
                ]
            },
            {
                identifier:2,
                text:"You really shouldn't...",
                options: [
                    { text: "default", node : 3 }
                ]
            },
            { identifier: 3, text: "...and won't." },
            { identifier: 4, text: "Good call." }
        ]
    },
    merchant : {
        nodes : [
            {
                identifier : 1,
                text : "Welcome to my shop. Take a look around."
            }
        ],
        inventory : [
            "sword/shortsword",
            "staff/firewand",
            "book/scroll",
            "gun/pistol",
            "unarmed/brassknuckles",
            "cloth/vest",
            "leather/jerkin",
            "mail/hauberk",
            "plate/breastplate"
        ]
    }
}