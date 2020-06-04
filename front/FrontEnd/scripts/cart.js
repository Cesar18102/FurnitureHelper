let CART = {};

CART.COOKIE = undefined;
CART.PROMISE_COOKIE = import('/FurnitureFrontEnd/scripts/cookie.js').then(module => CART.COOKIE = module);

async function getCart() {
	if(CART.COOKIE == undefined) {
		await CART.PROMISE_COOKIE;
	}
	
	let ordersCookie = CART.COOKIE.getCookie("order");
	return ordersCookie == undefined ? [ ] : JSON.parse(ordersCookie);
}

async function addToCartStore(position_info) {
	let orders = await getCart();
					
	let orderIndex = orders.findIndex(o => 
		o.part.id == position_info.part.id && 
		o.material.id == position_info.material.id && 
		o.color.id == position_info.color.id
	);
					
	if(orderIndex == -1) {
		orders.push(position_info);
	} else {
		orders[orderIndex].amount = amount; //+=
	}
	
	CART.COOKIE.setCookie("order", JSON.stringify(orders), { 'max-age': 8640000000000 });
}

async function clearCart() {
	CART.COOKIE.setCookie("order", "[ ]", { 'max-age': 8640000000000 });
}

async function removeFromCart(part_id, material_id, color_id) {
	let orders = await getCart();
					
	let orderIndex = orders.findIndex(o => 
		o.part.id == part_id && 
		o.material.id == material_id && 
		o.color.id == color_id
	);
	
	orders.splice(orderIndex, 1);
					
	CART.COOKIE.setCookie("order", JSON.stringify(orders), { 'max-age': 8640000000000 });
}