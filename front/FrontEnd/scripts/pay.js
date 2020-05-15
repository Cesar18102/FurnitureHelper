let PAY = {};

PAY.COOKIE = undefined;
PAY.PROMISE_COOKIE = import('./cookie.js').then(module => PAY.COOKIE = module);

async function getPaymentInfo() {
	if(PAY.COOKIE == undefined) {
		await PAY.PROMISE_COOKIE;
	}
	
	let payment = PAY.COOKIE.getCookie("PAYMENT");
	if(payment == undefined) {
		return null;
	}
	
	return JSON.parse(payment);
}

async function dropPaymentInfo() {
	if(PAY.COOKIE == undefined) {
		await PAY.PROMISE_COOKIE;
	}
	
	PAY.COOKIE.setCookie("PAYMENT", "", { 'max-age': -1 } ); 
}

async function setPaymentInfo(payment) {
	if(PAY.COOKIE == undefined) {
		await PAY.PROMISE_COOKIE;
	}
	
	PAY.COOKIE.setCookie("PAYMENT", JSON.stringify(payment), { 'max-age': 8640000000000 } ); 
}