const SERVER_URL = "http://192.168.0.108:5000/api/"; //"http://furniturehelper.somee.com/api/"; 
const CONTENT_TYPE_JSON = "application/json";

const POST_METHOD = "POST";
const GET_METHOD = "GET";

async function SendPostAsync(endpoint, body) {
	let uri = SERVER_URL + endpoint;
	let requestPromise = new Promise((resolve, reject) => {
		resolve(Send(uri, body, POST_METHOD, CONTENT_TYPE_JSON));
	});
	return await requestPromise;
}

async function SendGetAsync(endpoint, uriParams = []) {	
	let params = [];
	for(let key in uriParams) {
		params.push(key + "=" + uriParams[key]);
	}
	let paramString = params.join("&");
	let uri = SERVER_URL + endpoint + (paramString == "" ? "" : "?") + paramString;
	
	let requestPromise = new Promise((resolve, reject) => {
		resolve(Send(uri, null, GET_METHOD, CONTENT_TYPE_JSON));
	});
	
	return await requestPromise;
}

function Send(url, body, method, contentType) {
	let request = new XMLHttpRequest();
	request.open(method, url, false);
	
	request.setRequestHeader('Content-Type', contentType);
	request.send(body);
	
	let result = {
		status : {
			state : request.status,
			text : request.statusText
		},
		response : request.response
	};
	
	return result;
}