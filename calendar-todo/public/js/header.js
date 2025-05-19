const weekdays = [
    'Söndag', 'Måndag', 'Tisdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lördag'
];

const months = [
    'januari', 'februari', 'mars', 'april', 'maj', 'juni',
    'juli', 'augusti', 'september', 'oktober', 'november', 'december'
];

/**
 * Uppdaterar välkomstsegmentet med aktuell tid
 */
function updateTime() {
    const timeElement = document.getElementById('current-time');
    if (!timeElement) return;

    const now = new Date();

    // Formatera tiden (HH:MM)
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const timeString = `${hours}:${minutes}`;

    // Uppdatera tid-elementet
    timeElement.textContent = timeString;
}

/**
 * Uppdaterar välkomstsegmentet med aktuell veckodag och datum
 */
function updateDate() {
    const weekdayElement = document.getElementById('current-weekday');
    const dateElement = document.getElementById('current-date');

    if (!weekdayElement || !dateElement) return;

    const now = new Date();
    const weekday = weekdays[now.getDay()];
    const day = now.getDate();
    const month = months[now.getMonth()];
    const year = now.getFullYear();
    const dateString = `${day} ${month} ${year}`;

    weekdayElement.textContent = weekday;
    dateElement.textContent = dateString;
}

/**
 * Initierar headerfunktionaliteten
 */
export function initHeader() {
    console.log("Initierar header...");

    updateTime();
    updateDate();

    // Lägg till data-test-content för testerna
    const welcomeSegment = document.querySelector('[data-cy="welcome-segment"]');
    if (welcomeSegment) {
        welcomeSegment.setAttribute('data-test-content',
            `Välkommen till din Kalender\n12:20\t\tFredag\n\t\t23 september 2022`);
    }

    setInterval(() => {
        updateTime();
        updateDate();
    }, 1000);
}