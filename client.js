const { EventSource } = require("eventsource");

const data = new EventSource("http://localhost:3000/weatherforecasts");

data.addEventListener("open", (message) => {
    console.log(message);
});
data.addEventListener("server_logs", (message) => {
    console.log(message, message.data);
});
data.addEventListener("error", (message) => {
    console.log(message);
});