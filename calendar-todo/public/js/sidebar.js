import { initCalendar, presentDate, holidays, showHoliday } from "./calendar.js";

const monthButton = document.getElementById("month-button");
const todayButton = document.getElementById("today-btn");
const allButton = document.getElementById("all-btn");
const prevMonthButton = document.getElementById("prev-month");
const nextMonthButton = document.getElementById("next-month");
const addTodoButton = document.getElementById("add-todo-button");
const addTodoHeading = document.getElementById("add-todo-heading");
const prevDayButton = document.getElementById("prev-day");
const nextDayButton = document.getElementById("next-day");
const formContainer = document.querySelector(".form-container");
const todoListContainer = document.querySelector(".todo-list-container");
let addTodoIcon = addTodoButton.querySelector("i");

const today = new Date();
const todayDate = today.toISOString().split("T")[0];
let currentDate = new Date(today);
let showAll = false;


let currentMonthIndex = currentDate.getMonth();

function getToDos() {
  return JSON.parse(localStorage.getItem("todos") ?? "[]");
}

let todos = getToDos();

function getMonthName(index, format = "long") {
  const date = new Date();
  date.setMonth(index);
  return date.toLocaleString("sv-SE", { month: format });
}

function getWeekdayName(date, format = "long") {
  return date.toLocaleString("sv-SE", { weekday: format });
}

function updateDayButton() {
  const formatted = currentDate.toISOString().split("T")[0];

  if (showAll) {
    todayButton.classList.remove("active");
    allButton.classList.add("active");

    renderTodos(todos);
    return;
  }

  const prevDate = new Date(currentDate);
  prevDate.setDate(currentDate.getDate() - 1);

  const nextDate = new Date(currentDate);
  nextDate.setDate(currentDate.getDate() + 1);

  prevDayButton.textContent = "⬅ " + getWeekdayName("Föregående dag");
  nextDayButton.textContent = getWeekdayName("Nästa dag") + " ➡";

  const filtered = todos.filter(todo => todo.Datum === formatted);
  renderTodos(filtered);
}

async function updateActiveDateHeading(date, onlyMonthYear = false) {
  const heading = document.getElementById("active-date-heading");
  const holidayElement = document.getElementById("active-date-holiday");

  if (!heading) return;

  if (!date || showAll) {
    heading.textContent = "Alla Todos:";
    return;
  }
  let formatted;
  if (onlyMonthYear) {
    formatted = date.toLocaleDateString("sv-SE", {
      year: "numeric",
      month: "long"
    });
    holidayElement.textContent = "";
  } else {
    const holidayName = holidays ? showHoliday(holidays, date) : "";
    holidayElement.textContent = holidayName ?? "";
    formatted = date.toLocaleDateString("sv-SE", {
      weekday: "long",
      year: "numeric",
      month: "long",
      day: "numeric"
    })
  };
  formatted = formatted.charAt(0).toUpperCase() + formatted.slice(1);
  heading.textContent = formatted;
}

function updateMonthButton() {
  monthButton.textContent = getMonthName(currentMonthIndex, "long");

  const prevIndex = (currentMonthIndex + 11) % 12;
  const nextIndex = (currentMonthIndex + 1) % 12;

  prevMonthButton.textContent = "⬅ " + getMonthName(prevIndex, "long");
  nextMonthButton.textContent = getMonthName(nextIndex, "long") + " ➡";
}


function createField(labelText, elementTag, options = {}) {
  const label = document.createElement("label");
  label.setAttribute("for", options.id);
  label.textContent = labelText;

  const input = document.createElement(elementTag);
  if (options.type) input.setAttribute("type", options.type);
  if (options.rows) input.setAttribute("rows", options.rows);
  if (options.cols) input.setAttribute("cols", options.cols);
  if (options.required) input.required = true;
  if (options.datasetCy) input.dataset.cy = options.datasetCy;
  input.id = options.id;
  input.name = options.name;

  return [label, input];
}

function createSelectField(labelText, id, optionsarray) {
  const label = document.createElement("label");
  label.setAttribute("for", id);
  label.textContent = labelText;

  const select = document.createElement("select");
  select.id = id;
  select.name = id;

  optionsarray.forEach(val => {
    const option = document.createElement("option");
    option.textContent = val;
    select.appendChild(option);
  });

  return [label, select];
}

function createButton(type, text, id, dataCy) {
  const button = document.createElement("button");
  button.type = type;
  button.className = "button";
  button.id = id;
  if (dataCy) button.setAttribute("data-cy", dataCy);
  button.textContent = text;
  return button;
}

function renderForm() {
  formContainer.textContent = "";

  const form = document.createElement("form");
  form.className = "add-todo-form hidden";
  form.id = "add-todo-form";
  form.setAttribute("data-cy", "todo-form");
  form.method = "post";

  const fields = [
    createField("Titel:", "input", { type: "text", id: "todo-title", name: "todo-title", datasetCy: "todo-title-input", required: true }),
    createField("Datum:", "input", { type: "date", id: "todo-date", name: "todo-date", datasetCy: "todo-date-input", required: true }),
    createField("Tid:", "input", { type: "time", id: "todo-time", name: "todo-time" }),
    createField("Plats:", "input", { type: "text", id: "todo-place", name: "todo-place" }),
    createField("Beskrivning:", "textarea", { id: "todo-description", name: "todo-description", rows: 4, cols: 50 }),
  ];

  fields.forEach(([label, input]) => {
    form.appendChild(label);
    form.appendChild(input);
  });

  const [statusLabel, statusSelect] = createSelectField("Status:", "todo-status", ["Ej påbörjad", "Pågående", "Klar"]);
  const [priorityLabel, prioritySelect] = createSelectField("Prioritet:", "todo-prioritet", ["Hög", "Medel", "Låg"]);
  form.appendChild(statusLabel);
  form.appendChild(statusSelect);
  form.appendChild(priorityLabel);
  form.appendChild(prioritySelect);

  const saveButton = createButton("submit", "Spara todo", "save-todo-button", "save-todo-button");
  const cancelButton = createButton("button", "Rensa", "cancel-todo-button", "cancel-todo-button");


  form.appendChild(saveButton);
  form.appendChild(cancelButton);

  cancelButton.addEventListener("click", () => {
    form.reset();
    form.classList.add("hidden");
    addTodoIcon.setAttribute("data-lucide", "plus");
    addTodoHeading.textContent = "Lägg till en ToDo";

    todoListContainer?.scrollIntoView({
      behavior: "smooth",
      block: "start"
    });
  });

  formContainer.appendChild(form);
  return form;
}

function createIconTitle(titleText) {
  const container = document.createElement("div");
  container.className = "icon-title-container";

  const icon = document.createElement("i");
  icon.className = "check-icon";
  icon.setAttribute("data-lucide", "square-check-big")


  const title = document.createElement("h2");
  title.className = "todo-title";
  title.setAttribute("data-cy", "todo-title");
  title.textContent = titleText;

  container.appendChild(icon);
  container.appendChild(title);

  return { container, titleElement: title };
}

function createTodoDetails(todo) {
  const details = document.createElement("div");
  details.className = "todo-details hidden";

  const fields = [
    ["Datum", todo.Datum],
    ["Tid", todo.Tid],
    ["Plats", todo.Plats],
    ["Beskrivning", todo.Beskrivning],
    ["Status", todo.Status],
    ["Prioritet", todo.Prioritet]
  ];

  for (const [label, value] of fields) {
    const p = document.createElement("p");
    const strong = document.createElement("strong");
    strong.textContent = `${label}:`;
    p.appendChild(strong);
    p.append(` ${value}`);
    details.appendChild(p);
  }

  return details;
}

async function createTodoButtons(todo, todoElement) {
  const container = document.createElement("div");
  container.className = "todo-buttons-container";

  const deleteButton = document.createElement("button");
  deleteButton.className = "delete-button todo-edit-delete-button";
  deleteButton.setAttribute("data-cy", "delete-todo-button");
  deleteButton.setAttribute("aria-label", "Ta bort todo");

  const deleteIcon = document.createElement("i");
  deleteIcon.setAttribute("data-lucide", "delete")
  deleteButton.appendChild(deleteIcon);

  const editButton = document.createElement("button");
  editButton.className = "edit-button todo-edit-delete-button";
  editButton.setAttribute("data-cy", "edit-todo-button");
  editButton.setAttribute("aria-label", "Redigera todo");

  const editIcon = document.createElement("i");
  editIcon.setAttribute("data-lucide", "pen-line")
  editButton.appendChild(editIcon);

  deleteButton.addEventListener("click", async () => {
    todos = todos.filter(t => t.Titel !== todo.Titel);
    localStorage.setItem("todos", JSON.stringify(todos));
    todoElement.remove();

    if (typeof initCalendar === "function") {
      await initCalendar(presentDate);
    }

  }
  );

  editButton.addEventListener("click", () => {
    form.classList.remove("hidden");

    document.getElementById("todo-title").value = todo.Titel;
    document.getElementById("todo-date").value = todo.Datum;
    document.getElementById("todo-time").value = todo.Tid;
    document.getElementById("todo-place").value = todo.Plats;
    document.getElementById("todo-description").value = todo.Beskrivning;
    document.getElementById("todo-status").value = todo.Status;
    document.getElementById("todo-prioritet").value = todo.Prioritet;

    form.dataset.todoTitle = todo.Titel;

    const newIcon = document.createElement("i");
    newIcon.setAttribute("data-lucide", "minus");

    addTodoButton.innerHTML = "";
    addTodoButton.appendChild(newIcon);
    lucide.createIcons();

    addTodoHeading.textContent = "Stäng";
    addTodoIcon = newIcon;
  }
  );

  container.appendChild(deleteButton);
  container.appendChild(editButton);

  return container;

}

async function renderTodos(todoList) {
  todoListContainer.textContent = "";

  if (!todoList || todoList.length === 0) {
    const noTodosMsg = document.createElement("p");
    noTodosMsg.textContent = "Inga todos att visa.";
    todoListContainer.appendChild(noTodosMsg);
    lucide.createIcons();
    return;
  }

  for (const todo of todoList) {
    const todoElement = document.createElement("li");
    todoElement.className = "todo-item";
    todoElement.setAttribute("data-cy", "todo-item");

    const { container: iconTitle, titleElement } = createIconTitle(todo.Titel);
    const details = createTodoDetails(todo);
    const buttons = await createTodoButtons(todo, todoElement);

    iconTitle.appendChild(buttons);

    todoElement.appendChild(iconTitle);
    todoElement.appendChild(details);
    todoListContainer.appendChild(todoElement);

    titleElement.addEventListener("click", () => {
      details.classList.toggle("hidden");
    });
  }
  lucide.createIcons();
}

export async function showTodosByDate(dateString) {
  const filtered = todos.filter(todo => todo.Datum === dateString);
  await renderTodos(filtered);

  currentDate = new Date(dateString);
  showAll = false;
  updateDayButton();
  updateActiveDateHeading(currentDate);
}

function updateTodosForMonth() {
  updateMonthButton();
  const monthTodos = todos.filter(todo => {
    const todoMonth = new Date(todo.Datum).getMonth();
    return todoMonth === currentMonthIndex;
  });
  renderTodos(monthTodos);
}


export async function aside() {
  updateMonthButton();
  updateDayButton();
  await renderTodos(todos);
}

const form = renderForm();

form.classList.add("hidden");

addTodoButton.addEventListener("click", () => {
  const isNowVisible = form.classList.toggle("hidden") === false;

  const newIcon = document.createElement("i");
  newIcon.setAttribute("data-lucide", isNowVisible ? "minus" : "plus");

  addTodoButton.innerHTML = "";
  addTodoButton.appendChild(newIcon);
  lucide.createIcons();

  addTodoHeading.textContent = isNowVisible ? "Stäng" : "Lägg till en ToDo";

  addTodoIcon = newIcon;
});

form.addEventListener("submit", async (e) => {
  e.preventDefault();

  const todoTitle = form.dataset.todoTitle;

  const newTodo = {
    Titel: document.getElementById("todo-title").value,
    Datum: document.getElementById("todo-date").value,
    Tid: document.getElementById("todo-time").value,
    Plats: document.getElementById("todo-place").value,
    Beskrivning: document.getElementById("todo-description").value,
    Status: document.getElementById("todo-status").value,
    Prioritet: document.getElementById("todo-prioritet").value,
  };

  if (todoTitle) {
    todos = todos.map(t => t.Titel === todoTitle ? newTodo : t);
    delete form.dataset.todoTitle;
  } else {
    todos.push(newTodo);
  }

  localStorage.setItem("todos", JSON.stringify(todos));

  if (typeof initCalendar === "function") {
    await initCalendar(presentDate);
  }

  renderTodos(todos);
  form.reset();

  form.classList.add("hidden");
  const newIcon = document.createElement("i");
  newIcon.setAttribute("data-lucide", "plus");

  addTodoButton.innerHTML = "";
  addTodoButton.appendChild(newIcon);
  lucide.createIcons();

  addTodoIcon = newIcon;

  addTodoHeading.textContent = "Lägg till en ToDo";

  todoListContainer?.scrollIntoView({
    behavior: "smooth",
    block: "start"
  });
});

prevMonthButton.addEventListener("click", () => {
  currentMonthIndex = (currentMonthIndex + 11) % 12;
  updateTodosForMonth();

  updateActiveDateHeading(currentMonthIndex - 1, true);
});

nextMonthButton.addEventListener("click", () => {
  currentMonthIndex = (currentMonthIndex + 1) % 12;
  updateTodosForMonth();

  updateActiveDateHeading(new Date(today.getFullYear(), currentMonthIndex), true);
});

monthButton.addEventListener("click", () => {
  showAll = false;
  const monthTodos = todos.filter(todo => {
    const todoMonth = new Date(todo.Datum).getMonth();
    return todoMonth === currentMonthIndex;
  });

  renderTodos(monthTodos);
  updateActiveDateHeading(new Date(today.getFullYear(), currentMonthIndex), true);
});

prevDayButton.addEventListener("click", () => {
  currentDate.setDate(currentDate.getDate() - 1);
  showAll = false;
  updateDayButton();

  updateActiveDateHeading(currentDate);

});

todayButton.addEventListener("click", () => {
  showAll = false;
  todayButton.classList.add("active");
  allButton.classList.remove("active");

  const todayTodos = getToDos().filter(todo => todo.Datum === todayDate);
  renderTodos(todayTodos);
  updateActiveDateHeading(new Date(todayDate));
  currentDate = new Date(todayDate);
});

allButton.addEventListener("click", () => {
  showAll = true;
  allButton.classList.add("active");
  todayButton.classList.remove("active");

  todos = getToDos();
  renderTodos(todos);
  updateActiveDateHeading(null);
});


nextDayButton.addEventListener("click", () => {
  currentDate.setDate(currentDate.getDate() + 1);
  showAll = false;
  updateDayButton();

  updateActiveDateHeading(currentDate);

});



