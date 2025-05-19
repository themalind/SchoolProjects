import { initCalendar, presentDate } from "./calendar.js"
import { aside } from "./sidebar.js"; 
import { initHeader } from "./header.js"; 

async function main() {
 initHeader();
 await initCalendar(presentDate);
 await aside();
}

window.addEventListener("DOMContentLoaded", async () => {
    await main();
});










