document.addEventListener("DOMContentLoaded", function() {
    const chatBox = document.getElementById("chat-box");
    const userInput = document.getElementById("user-input");
    const sendButton = document.getElementById("send-button");

    function addMessage(sender, message) {
        const messageDiv = document.createElement("div");
        messageDiv.classList.add(sender === "user" ? "user-message" : "bot-message");
        messageDiv.innerHTML = message;
        chatBox.appendChild(messageDiv);
        chatBox.scrollTop = chatBox.scrollHeight;
    }

    sendButton.addEventListener("click", async function() {
        const userMessage = userInput.value;
        addMessage("user","You: "+userMessage);
        userInput.value = "";

        // Handle predefined responses based on user input
        let botResponse = "Bot: I'm sorry, I don't have an answer for that.";
        if (userMessage.toLowerCase() === "hello") {
            botResponse = "Bot: Hello! Howww can I assist you today?";
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

        const response = await fetch('http://localhost:5037/api/rules/ruleshindi'+userMessage);
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