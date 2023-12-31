document.addEventListener("DOMContentLoaded", function () {
    const chatBox = document.getElementById("chat-box");
    const userInput = document.getElementById("user-input");
    const sendButton = document.getElementById("send-button");

    function addMessage(sender, message) {
        const messageDiv = document.createElement("div");
        messageDiv.classList.add(sender === "user" ? "user-message" : "bot-message");

        if (sender === "user") {
            messageDiv.textContent = message;
            chatBox.appendChild(messageDiv);
        } else {
            const temporaryElement = document.createElement('div');
            temporaryElement.innerHTML = message;
            const text = temporaryElement.textContent;
            const paragraphs = text.split('\\n'); // Split text into paragraphs
            console.log(paragraphs.length)
            console.log(message)

            // paragraphs.forEach(async (paragraph, index) => {
            //     if (index !== 0) {
            //         const paragraph1Div = document.createElement('div');
            //         paragraph1Div.classList.add('paragraph1');
            //         messageDiv.appendChild(paragraph1Div);
            //         await animateTyping(paragraph, paragraph1Div)
            //     } else {
            //         const paragraphDiv = document.createElement('div');
            //         messageDiv.appendChild(paragraphDiv);
            //         paragraphDiv.textContent = paragraph
            //     };
            // }
            // );


            // chatBox.appendChild(messageDiv);
            let first=true;
            async function typeNextParagraph(index) {
                const paragraphDiv = document.createElement('div');
                if (index < paragraphs.length) {
                    // if (messageDiv.innerHTML !== "") {
                    //     messageDiv.innerHTML += "<br>"; // Add a line break if not the first paragraph
                    // }
                    
                    if (index>0){
                        paragraphDiv.classList.add('paragraph1');
                    }
                    messageDiv.appendChild(paragraphDiv);
                    await animateTyping(paragraphs[index], paragraphDiv);
                    chatBox.appendChild(messageDiv);
                    typeNextParagraph(index + 1); // Recursively type the next paragraph   
                } 
                if (index == paragraphs.length-1){
                    for (const id in idToTitleMap) {
                        if (idToTitleMap.hasOwnProperty(id)&&!first) {
                            const title = idToTitleMap[id];
                            const link = document.createElement("a");
                            link.classList.add('button')
                            // const elementsToDelete = document.querySelectorAll(".button");
                            const apiEndpoint="http://localhost:5037/api/rules/"
                            link.textContent = title;
                            link.addEventListener("click", function() {
                                // Handle the click event by making an API request with the ID
                                const apiRequestUrl = apiEndpoint+id;
                                // Perform your API request here, e.g., using fetch
                                fetch(apiRequestUrl)
                                    .then(response => response.json())
                                    .then(data => {
                                        // Handle the API response data
                                        console.log(data);
                                        delete idToTitleMap[data.ruleId]
                                        addMessage("bot",data.title+"\\n"+data.content+"\\n"+
                                        "Here are the other related rule/s based on your query:\\n")
                                    })
                                    .catch(error => {
                                        console.error('Error fetching API data:', error);
                                    });
                            });
                            paragraphDiv.appendChild(link);
                           
                        }else{
                            first=false;
                        }
                    } 
                }
                
                // else {
                //     chatBox.appendChild(messageDiv); // Add the fully typed message to the chatBox
                // }
            }
            typeNextParagraph(0);
            
            // for (const id in idToTitleMap) {
            //     if (idToTitleMap.hasOwnProperty(id)) {
            //         const title = idToTitleMap[id];
            //         const link = document.createElement("a");
            //         link.classList.add('button')
            //         // const elementsToDelete = document.querySelectorAll(".button");
            //         const apiEndpoint="http://localhost:5037/api/rules/"
            //         link.textContent = title;
            //         link.addEventListener("click", function() {
            //             // Handle the click event by making an API request with the ID
            //             const apiRequestUrl = apiEndpoint+id;
            //             // Perform your API request here, e.g., using fetch
            //             fetch(apiRequestUrl)
            //                 .then(response => response.json())
            //                 .then(data => {
            //                     // Handle the API response data
            //                     console.log(data);
            //                     delete idToTitleMap[data.ruleId]
            //                     addMessage("bot",data.title+"\\n"+data.content)
            //                 })
            //                 .catch(error => {
            //                     console.error('Error fetching API data:', error);
            //                 });
            //         });
            //         paragraphDiv.appendChild(link);
                   
            //     }
            // }
        }
    }

    async function animateTyping(message, messageDiv) {
        return new Promise(resolve => {
        let currentIndex = 0;

        const typingInterval = setInterval(function () {
            if (currentIndex < message.length) {
                messageDiv.innerHTML += message.charAt(currentIndex);
                currentIndex++;
            } else {
                clearInterval(typingInterval);
                chatBox.scrollTop = chatBox.scrollHeight;
                resolve(); // Resolve the promise when typing is complete
            }
        }, 15); // Adjust the typing speed by changing the interval (e.g., 50 milliseconds for a faster typing effect)
    });}

    sendButton.addEventListener("click", async function () {
        for (const key in idToTitleMap) {
            if (idToTitleMap.hasOwnProperty(key)) {
                delete idToTitleMap[key];
            }
        }
        const userMessage = userInput.value;
        addMessage("user", userMessage);
        userInput.value = "";

        // Handle predefined responses based on user input
        let botResponse = "Bot: I'm sorry, I don't have an answer for that.";
        if (userMessage.toLowerCase() === "hello") {
            botResponse = "Bot: Hello! How can I assist you today?";
        } else if (userMessage.toLowerCase() === "rule 5") {
            botResponse = "Bot: To be eligible for appointment by direct selection to the post mentioned in rule 3, a candidate shall, <br/>(a) not be more than 35 years of age: <br/>Provided that the upper age limit may be relaxed in favour of a candidate who is already in the service of the Government of Gujarat in accordance with the provisions of the Gujarat Civil Services Classification and Recruitment (General) Rules, 1967; <br/>(b) possess a post graduate degree with at least 55% marks in Statistics or Applied Statistics or Mathematical Statistics or Economics or Applied Economics or Business Economics or Econometrics or Mathematics as principal subject from any of the Universities established or incorporated by or under the Central or State Act in India; or any other educational institution recognised such or declared as deemed to be University under section 3 of the University Grants Commission Act, 1956";
        } else if (userMessage.toLowerCase() === "rule xyz") {
            botResponse = "Bot: Here is the information for rule XYZ.";
        } else if (userMessage.toLowerCase() === "goodbye") {
            botResponse = "Bot: Goodbye! Have a great day!";
        } else {
            botResponse = "Bot: " + await displayAPIResponseInChatbot(userMessage);
        }

        addMessage("bot", botResponse+"\\n"+
        "Here are the other related rule/s based on your query:\\n");
    });

    userInput.addEventListener("keyup", function (event) {
        if (event.key === "Enter") {
            sendButton.click();
        }
    });
});
const idToTitleMap = {};
async function displayAPIResponseInChatbot(userMessage) {
    try {
        // Display a loading message in the chatbox while waiting for the response.
        // const chatBox = document.getElementById("chat-box");
        // chatBox.innerHTML = 'Fetching data...';

        const response = await fetch('http://localhost:5037/api/rules/rulesmulti' + userMessage);
        const data = await response.json();
        const title = "<b>" + data[0].title + "</b>\\n";
        const content = data[0].content;
        const botResponse = title + "<br>" + content + "<br>\\n";
        console.log(data.length);

        // Populate the mapping object
        data.forEach(item => {
            idToTitleMap[item.ruleId] = item.title;
        });
        // console.log(idToTitleMap[24])
        for (const id in idToTitleMap) {
            if (idToTitleMap.hasOwnProperty(id)) {
                const title = idToTitleMap[id];
                console.log(`ID: ${id}, Title: ${title}`);
            }
        }
        
        return botResponse;
    } catch (error) {
        // idToTitleMap = {};
        const botResponse="No related rule found!";
        console.error('Error fetching API data:', error);
        return botResponse;
    }
}