function createPopup(params) {
	let body = document.createElement("div");
	body.classList.add(params.bodyStyle);
	body.innerHTML = params.content;
	
	let wrapper = document.createElement("div");
	wrapper.classList.add(params.wrapperStyle);
	wrapper.appendChild(body);
	
	document.getElementById(params.holder).appendChild(wrapper);
	params.vue();
	
	body.onclick = e => {
		e.preventDefault();
		e.stopPropagation();
	}
	
	wrapper.show = () => wrapper.hidden = false;
	wrapper.hide = () => wrapper.hidden = true;
	
	wrapper.onclick = e => {
		wrapper.hide();
	};
	
	body.style.marginTop = (window.innerHeight - body.offsetHeight) / 2 + "px";
	wrapper.hidden = true;
	
	return wrapper;
}