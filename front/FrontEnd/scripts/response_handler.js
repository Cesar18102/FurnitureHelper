function handleResponse(result, errorHandler, successHandler) {
	let response = JSON.parse(result.response);
	if(response.error != null) {
		errorHandler(response.error);
	} else {
		successHandler(response.data);
	}
}