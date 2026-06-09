import type {
    TodoResponse
} from "../../../src/types/todo";
import {
    getTodos,
    createTodo,
    updateTodo,
    deleteTodo
} from "../../../src/api/todoApi";

// apiから帰ってきたTodosをキャッシュしておく
let cachedTodos:TodoResponse[] = [];

// DOM要素取得
// フォーム
const todoForm = document.getElementById("todoForm") as HTMLFormElement;
const titleInput = document.getElementById("title") as HTMLInputElement;
const descriptionInput = document.getElementById("description") as HTMLInputElement;

// Todo一覧表示用のテーブル
const todoTableBody = document.getElementById("todoTableBody") as HTMLTableSectionElement;

// Modal要素
// Modal詳細表示用の要素
const detailModal = document.getElementById("detailModal") as HTMLDivElement;
const modalTitle = document.getElementById("modalTitle") as HTMLSpanElement;
const modalDescription = document.getElementById("modalDescription") as HTMLSpanElement;
const modalSummary = document.getElementById("modalSummary") as HTMLSpanElement;
const modalCreatedAt = document.getElementById("modalCreatedAt") as HTMLSpanElement;
const modalUpdatedAt = document.getElementById("modalUpdatedAt") as HTMLSpanElement;
// Modal更新用の要素
const modalUpdateForm = document.getElementById("modalUpdateForm") as HTMLFormElement;
const modalUpdateIdInput = document.getElementById("modalUpdateId") as HTMLInputElement;
const modalUpdateTitleInput = document.getElementById("modalUpdateTitle") as HTMLInputElement;
const modalUpdateDescriptionInput = document.getElementById("modalUpdateDescription") as HTMLInputElement;
const modalUpdateIsCompletedInput = document.getElementById("modalUpdateIsCompleted") as HTMLInputElement;
// Modalボタン
const modalDeleteButton = document.getElementById("modalDeleteButton") as HTMLButtonElement;
const modalCloseButton = document.getElementById("modalCloseButton") as HTMLButtonElement;

// Logoutボタン
const logoutButton = document.getElementById("logoutButton") as HTMLButtonElement;

// 日付フォーマットあとでutilsに切り出す
function formatDate(isoString: string) {
    const date = new Date(isoString);
    return date.toLocaleString("ja-JP");
}
// TodoTableBodyのtd作成あとでどこかに切り出す
function createTodoCell(todo: TodoResponse): HTMLTableRowElement {
    const tr = document.createElement("tr");

    const titleTd = document.createElement("td");
    titleTd.textContent = todo.title;
    titleTd.classList.add("px-4", "py-2");
    tr.appendChild(titleTd);

    const isCompletedTd = document.createElement("td");
    isCompletedTd.textContent = todo.isCompleted ? "完了" : "未完了";
    isCompletedTd.classList.add("px-4", "py-2", "font-semibold", todo.isCompleted ? "text-green-600" : "text-red-600");
    tr.appendChild(isCompletedTd);

    const createdAtTd = document.createElement("td");
    createdAtTd.textContent = formatDate(todo.createdAt);
    createdAtTd.classList.add("px-4", "py-2");
    tr.appendChild(createdAtTd);


    const detailButtonTd = document.createElement("td");
    const detailButton = document.createElement("button");
    detailButton.textContent = "詳細";
    detailButton.classList.add("bg-blue-500", "text-white", "px-4", "py-1", "rounded", "hover:bg-blue-600", "transition-colors", "cursor-pointer");
    // 詳細ボタンにModalを開くイベントを持たせる
    detailButton.addEventListener("click", ()=>{
        openModal(todo.id);
    })
    detailButtonTd.appendChild(detailButton);
    tr.appendChild(detailButtonTd);
    tr.classList.add("hover:bg-gray-50", "transition-colors")

    return tr;
}

// Todo一覧を取得し描画
async function loadTodos() {
    cachedTodos = await getTodos();

    todoTableBody.innerHTML = ""; // 既存の内容をクリア
    cachedTodos.forEach(todo => {
        const tr = createTodoCell(todo);
        todoTableBody.appendChild(tr);
    });
};

// Modalを開く
function openModal(id: number) {
    const detailTodo = cachedTodos.find(t => t.id == id);
    if (!detailTodo) {
        return;
    }

    // Modal詳細用
    modalTitle.textContent = detailTodo.title;
    modalDescription.textContent = detailTodo.description ?? "";
    modalSummary.textContent = detailTodo.summary ?? "";
    modalCreatedAt.textContent = formatDate(detailTodo.createdAt);
    modalUpdatedAt.textContent = formatDate(detailTodo.updatedAt);

    // Modal更新用
    modalUpdateIdInput.value = String(detailTodo.id);
    modalUpdateTitleInput.value = detailTodo.title;
    modalUpdateDescriptionInput.value = detailTodo.description ?? "";
    modalUpdateIsCompletedInput.checked = detailTodo.isCompleted;

    detailModal.classList.remove("hidden");
    detailModal.classList.add("flex");
}
// Modalを閉じる
modalCloseButton.addEventListener("click", () => {
    detailModal.classList.add("hidden");
    detailModal.classList.remove("flex");
});

// Todo追加
todoForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    await createTodo({
        title: titleInput.value,
        description: descriptionInput.value
    });

    titleInput.value = "";
    descriptionInput.value = "";

    await loadTodos();
});

// Todo更新
modalUpdateForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const detailTodo = await updateTodo(Number(modalUpdateIdInput.value), {
        title: modalUpdateTitleInput.value,
        description: modalUpdateDescriptionInput.value,
        isCompleted: modalUpdateIsCompletedInput.checked
    });

    // Modal詳細用
    modalTitle.textContent = detailTodo.title;
    modalDescription.textContent = detailTodo.description ?? "";
    modalSummary.textContent = detailTodo.summary ?? "";
    modalCreatedAt.textContent = formatDate(detailTodo.createdAt);
    modalUpdatedAt.textContent = formatDate(detailTodo.updatedAt);

    // Modal更新用
    modalUpdateIdInput.value = String(detailTodo.id);
    modalUpdateTitleInput.value = detailTodo.title;
    modalUpdateDescriptionInput.value = detailTodo.description ?? "";
    modalUpdateIsCompletedInput.checked = detailTodo.isCompleted;

    await loadTodos();
});

// Todo削除
modalDeleteButton.addEventListener("click", async () => {
    await deleteTodo(Number(modalUpdateIdInput.value));
    detailModal.classList.add("hidden");
    detailModal.classList.remove("flex");
    await loadTodos();
});

// Logout
logoutButton.addEventListener("click", () => {
    localStorage.removeItem("token");
    window.location.href = "/src/pages/login/login.html";
});

// 初期ロード
await loadTodos();