let SESSION = {};

SESSION.COOKIE = undefined;
SESSION.PROMISE_COOKIE = import('./cookie.js').then(module => SESSION.COOKIE = module);

SESSION.SHA256 = undefined;
SESSION.PROMISE_SHA256 = import('./libs/sha256.js').then(module => SESSION.PROMISE_SHA256 = module);

async function getSessionDto() {
	if(SESSION.COOKIE == undefined) {
		await SESSION.PROMISE_COOKIE;
	}
	
	if(SESSION.SHA256 == undefined) {
		await SESSION.PROMISE_SHA256;
	}
	
	let user_id = SESSION.COOKIE.getCookie("USER_ID");
	let token = SESSION.COOKIE.getCookie("SESSION_TOKEN");
	
	if(user_id == undefined || token == undefined) {
		return {
			user_id : 0,
			session_token_salted : "",
			salt : ""
		};
	}
	
	let salt = Math.round(Math.random() * 100000).toString();
	let token_salted = sha256(token + salt);
		
	let sessionDto = {
		user_id : user_id,
		session_token_salted : token_salted,
		salt : salt
	};
	return sessionDto;
}

async function createSession(user_id, token) {
	if(SESSION.COOKIE == undefined) {
		await SESSION.PROMISE_COOKIE;
	}
	
	SESSION.COOKIE.setCookie("USER_ID", user_id, { 'max-age': 8640000000000 } ); 
	SESSION.COOKIE.setCookie("SESSION_TOKEN", token, { 'max-age': 8640000000000 } ); 
}