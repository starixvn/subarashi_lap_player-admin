export const encode = (text) => {
    const p = document.createElement("p");
    p.textContent = text;

    return p.innerHTML;
}

export const decode = (html) => {
    const txt = document.createElement('textarea');
    txt.innerHTML = html;

    return txt.value;
}