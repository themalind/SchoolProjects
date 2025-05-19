import { showTodosByDate } from "./sidebar.js";
export let presentDate = new Date();

let todos;
export let holidays;
let holidaysSeen = new Map();

const weekDays = ["Söndag", "Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag", "Lördag"];

const months = [
  "Januari",
  "Februari",
  "Mars",
  "April",
  "Maj",
  "Juni",
  "Juli",
  "Augusti",
  "September",
  "Oktober",
  "November",
  "December",
];

async function getHolidays(year, month) {
  const url = `https://sholiday.faboul.se/dagar/v2.1/${year}/${month+1}`;
  try {
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }
    const json = await response.json();
   
    return json;

  } catch (error) {
    console.error(error.message);
  }
}

function totalDaysInMonth(month, year) {
  return new Date(year, month + 1, 0).getDate(); // För att få sista dagen i föregående månad.
}

function firstDayInMonthIndex(month, year) {
  return new Date(year, month, 1).getDay();
}

function printWeekdayNames(weekDays) {
  for (let index = 1; index < weekDays.length; index++) {
    const day = document.createElement("div");
    day.className = "calendar-weekday";
    day.innerHTML = `<p>${weekDays[index].at(0)}</p>`;
    document.querySelector(".calendar-container").append(day);
  }

  const sunday = document.createElement("div");
  sunday.className = "calendar-weekday";
  sunday.innerHTML = `<p>${weekDays[0].at(0)}</p>`;
  document.querySelector(".calendar-container").append(sunday);
}

function clearCalender() {
  document.querySelector(".calendar-month").innerHTML = "";
  document.querySelector(".calendar-container").innerHTML = "";
}

function printCalendarMonthAndYear(month, year) {
  const monthH2 = document.createElement("h3");
  monthH2.textContent = `${months[month]} ${year}`;
  document.querySelector(".calendar-month").append(monthH2);
}

function fillWithEmptyStartCells(weekday) {
  // Om första dagen i månaden är en söndag
  if (weekday === 0) {
    for (let index = 0; index < 6; index++) {
      const emptyDiv = document.createElement("div");
      emptyDiv.setAttribute("data-cy", "calendar-cell");
      document.querySelector(".calendar-container").append(emptyDiv);
    }
  } else {
    // index börjar på 1 som är Måndag i kalendens gränssnitt.
    for (let index = 1; index < weekday; index++) {
      const emptyDiv = document.createElement("div");
      emptyDiv.setAttribute("data-cy", "calendar-cell");
      document.querySelector(".calendar-container").append(emptyDiv);
    }
  }
}

function createCalendarButtons(totalDays, nowDateWithoutTime) {
  for (let index = 1; index <= totalDays; index++) {
    const newDate = new Date(presentDate.getFullYear(), presentDate.getMonth(), index);
    const dateString =  `${presentDate.getFullYear()}-${presentDate.getMonth() + 1 < 10 ? "0" : ""}${presentDate.getMonth() + 1}-${
          index < 10 ? "0" : "" }${index}`;

    const holiday = holidays.dagar.find(h => h.datum === dateString);
    const todayTodos = todos.filter((t) => t.Datum === dateString);

    const button = document.createElement("button");
    button.setAttribute("data-cy", "calendar-cell");

    const emptySpan = document.createElement("span");
    const dateSpan = document.createElement("span");
    dateSpan.setAttribute("data-cy", "calendar-cell-date");
    dateSpan.innerHTML = index;
 
    const todoSpan = document.createElement("span");
    const iconSpan = document.createElement("span");
        
    if (newDate < nowDateWithoutTime) {
      button.className = "beforinger";
    }
    if(holiday && holiday["röd dag"] === "Ja"){
      dateSpan.className ="holiday-button";
    }
    if(holiday && holiday.helgdagsafton || holiday && holiday.helgdag){
      const holidayName = holiday.helgdagsafton || holiday.helgdag;
        emptySpan.innerHTML = `<span data-cy="calendar-cell-holiday" class="holiday-span">${holidayName}</span>`;      
    }  
    if (todayTodos.length > 0) {  
      todoSpan.setAttribute("data-cy", "calendar-cell-todos");    
      todoSpan.className = "todoSpan";    
      todoSpan.innerHTML = `<span>${todayTodos.length}</span>`; 
      iconSpan.className = "todoSpan";
      iconSpan.innerHTML = `<div class="todos-icon"><span class="material-symbols-outlined">checklist</span></div> `;      
    }
    button.appendChild(emptySpan);
    button.appendChild(dateSpan);
    button.appendChild(todoSpan);
    button.appendChild(iconSpan);

    button.addEventListener("click", async () => {
      await showTodosByDate(dateString);
    });

    document.querySelector(".calendar-container").append(button);
  }
}

function fillCalendar(month, year) {
  const weekday = firstDayInMonthIndex(month, year);
  const totalDays = totalDaysInMonth(month, year);
  const nowWithTime = new Date();
  const nowDateWithoutTime = new Date(nowWithTime.getFullYear(), nowWithTime.getMonth(), nowWithTime.getDate());

  fillWithEmptyStartCells(weekday);
  createCalendarButtons(totalDays, nowDateWithoutTime);
}

async function addHolidays(){
  if(!holidaysSeen.has(`${presentDate.getFullYear()}-${ presentDate.getMonth()}`)){
     holidays = await getHolidays(presentDate.getFullYear(), presentDate.getMonth());
     holidaysSeen.set(`${presentDate.getFullYear()}-${ presentDate.getMonth()}`, holidays);
  }
  holidays = holidaysSeen.get(`${presentDate.getFullYear()}-${ presentDate.getMonth()}`); 
}

export function showHoliday(holidays, date)
{ 
  // Gippyhjälp för att formatera hemska datumet
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  const datestring = `${year}-${month}-${day}`;

  const holiday = holidays.dagar.find(h => h.datum === datestring );

  if(holiday && holiday.helgdagsafton || holiday && holiday.helgdag){
      const holidayName = holiday.helgdagsafton || holiday.helgdag;
        return `${holidayName}`;      
    }  
}

export async function initCalendar(date) {
  todos = JSON.parse(localStorage.getItem("todos") ?? "[]");
  clearCalender();
  await addHolidays();
  printCalendarMonthAndYear(date.getMonth(), date.getFullYear());
  printWeekdayNames(weekDays);
  fillCalendar(date.getMonth(), date.getFullYear());
}

export async function nextMonth() {
  presentDate.setMonth(presentDate.getMonth() + 1);

  await initCalendar(presentDate);
}

export async function previousMonth() {
  presentDate.setMonth(presentDate.getMonth() - 1);

  await initCalendar(presentDate);
}

document.getElementById("next").addEventListener("click", nextMonth);
document.getElementById("prev").addEventListener("click", previousMonth);
