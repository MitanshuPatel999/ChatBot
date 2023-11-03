document.addEventListener("DOMContentLoaded", function() {
    const chatBox = document.getElementById("chat-box");
    const userInput = document.getElementById("user-input");
    const sendButton = document.getElementById("send-button");

    function addMessage(sender, message) {
        const messageDiv = document.createElement("div");
        messageDiv.classList.add(sender === "user" ? "user-message" : "bot-message");

        // Add user message instantly
        if (sender === "user") {
            messageDiv.innerHTML = message;
            chatBox.appendChild(messageDiv);
        } else {
            chatBox.appendChild(messageDiv);

            // Animate the typing of the bot's message
            animateTyping(message, messageDiv);
        }
    }

    function animateTyping(message, messageDiv) {
        let currentIndex = 0;

        const typingInterval = setInterval(function() {
            if (currentIndex < message.length) {
                messageDiv.innerHTML += message.charAt(currentIndex);
                currentIndex++;
            } else {
                clearInterval(typingInterval);
                chatBox.scrollTop = chatBox.scrollHeight;
            }
        }, 50); // Adjust the typing speed by changing the interval (e.g., 50 milliseconds for a faster typing effect)
    }

    sendButton.addEventListener("click",async function() {
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
            botResponse="Bot: "+await displayAPIResponseInChatbot(userMessage);
        }

        addMessage("bot", botResponse);
    });

    userInput.addEventListener("keyup", function(event) {
        if (event.key === "Enter") {
            sendButton.click();
        }
    });
});

async function displayAPIResponseInChatbot(userMessage) {
    try {
        // Display a loading message in the chatbox while waiting for the response.
        // const chatbox = document.getElementById('chatbox');
        // chatbox.innerHTML = 'Fetching data...';

        const response = await fetch('http://localhost:5037/api/rules/searchrules'+userMessage);
        const data = await response.json();
        const title = "<b>"+data[0].title+"</b>";
        const content = data[0].content;
        const botResponse=title+"<br>"+content+"<br>";
            // botResponse = JSON.stringify(data, null, 2);
        return botResponse;
    } catch (error) {
        console.error('Error fetching API data:', error);
    }
}