const SERVER_URL = "http://localhost:5000/api/";
const CONTENT_TYPE_JSON = "application/json";

const POST_METHOD = "POST";
const GET_METHOD = "GET";

export async function SendPostAsync(endpoint, body, callback)
{
	let requestPromise = new Promise((resolve, reject) => {
		resolve(Send(endpoint, body, POST_METHOD, CONTENT_TYPE_JSON));
	});
	return await requestPromise;
}

export async function SendGetAsync(endpoint, uriParams, callback)
{
	let uri = endpoint + "?" + uriParams.map((key, value) => key + "=" + value).join("&");
	let requestPromise = new Promise((resolve, reject) => {
		resolve(Send(uri, null, GET_METHOD, CONTENT_TYPE_JSON));
	});
	return await requestPromise;
}

async function Send(url, body, method, contentType)
{
	let request = new XMLHttpRequest();
	request.setRequestHeader('Content-Type', contentType);
	request.open(method, url, false);
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