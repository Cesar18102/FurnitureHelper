SERVER_URL = "http://109.86.209.135:5000/api/";
APP_JSON_CONTENT_TYPE = 'Content-Type: application/json\r\n';

function connectWPS(connectedCallback, failedCallback)
  wifi.setmode(wifi.STATION);
  wps.enable();
    
  wifi.eventmon.register(
    wifi.eventmon.STA_GOT_IP, 
    function(connInfo)
      connectedCallback(connInfo);
    end
  );
    
  wps.start(
    function(status)
      if status == wps.SUCCESS then
        wps.disable();
        print("WPS: Success, connecting to AP...");
        wifi.sta.connect();   
      else
        failedCallback(status);
      end
      wps.disable();
    end
  );
end

function buildUri(url, params)
  if(params == nil) then return url; end
  
  paramString = "";
  for k, v in pairs(params) do
    paramString = paramString..k.."="..v.."&";
  end
  paramString = strsub(paramString, 1, strlen(paramString) - 1);
  
  if not paramString then
    return url.."?"..paramString;
  end
  
  return url;
end

function sendGet(url, params, successCallback, errorCallback)
  uri = buildUri(url, parameters);
  http.request(
    uri, nil, 
    function(code, data)
      if(code < 0) then
        errorCallback(code);
      else
        successCallback(data);
      end
    end
  )
end

function sendPost(url, contentType, body, successCallback, errorCallback)
  http.post(
    uri, contentType, body,
    function(code, data)
      if(code < 0) then
        errorCallback(code);
      else
        successCallback(data);
      end
    end
  )
end

function sendPostJson(url, body, successCallback, errorCallback)
  sendPost(url, APP_JSON_CONTENT_TYPE, body, successCallback, errorCallback);
end

connectWPS(
  function(connInfo)
    print("CONNECTED, IP: "..connInfo.IP);
    sendGet(
      SERVER_URL + "part/get", nil, 
      function(data) print(data) end,
      function(error) print(error) end
    );
  end,
  function(status)
    print("CONNECTION FAILED: "..status);
    exit();
  end
)
