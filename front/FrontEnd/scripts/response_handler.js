function handleResponse(result, errorHandler, successHandler) {
	let response = JSON.parse(result.response);
	if(response.error != null) {
		let err = response.error;
		err.code = result.status.state;
		errorHandler(err);
	} else {
		successHandler(response.data);
	}
}