--SERVER_URL = "http://furniturehelper.somee.com/api/";
SERVER_URL = "http://192.168.0.108:5000/api/";
APP_JSON_CONTENT_TYPE = 'Content-Type: application/json\r\n';
MAC = string.upper(wifi.sta.getmac());
print(MAC)

INDICATORS = {};

READERS = {};
READERS_OLD = {};
READERS_ROLLBACK = {};

REQUEST_ERROR_DELAY = 1000;
SERVER_PING_INTERVAL = 2000;
READ_PINS_INTERVAL = 3000;

STATUS_DONE = 0;
STATUS_ERROR = 1;
STATUS_PENDING = 2;
STATUS_FINISHED = 3;

function wps_connect(connected_callback, failed_callback)
  wifi.setmode(wifi.STATION);
  wifi.sta.setmac(MAC);
  wps.enable();
    
  wifi.eventmon.register(
    wifi.eventmon.STA_GOT_IP, 
    function(connection_info)
      connected_callback(connection_info);
    end
  );
    
  wps.start(
    function(status)
      if status == wps.SUCCESS then
        wps.disable();
        print("WPS: Success, connecting to AP...");
        wifi.sta.connect();   
      else
        failed_callback(status);
      end
      wps.disable();
    end
  );
end

function connect_access_point(ssid, pwd, connected_callback, failed_callback)
    wifi.setmode(wifi.STATION);
    local station_config = { ssid = ssid, pwd = pwd, auto = false };

    if not wifi.sta.config(station_config) then
      failed_callback("connection failed");
      return;
    end

    wifi.eventmon.register(
      wifi.eventmon.STA_GOT_IP, 
      function(connection_info)
        connected_callback(connection_info);
      end
    );

    wifi.sta.connect();
end

function set_interval(function_to_repeat, interval)
  local timer = tmr.create();
  timer:register(
    interval, tmr.ALARM_AUTO,
    function(timer) 
      function_to_repeat();
    end
  );
  timer:start();
end

function set_timeout(function_to_timeout, timeout)
  local timer = tmr.create();
  timer:register(
    timeout, tmr.ALARM_SINGLE,
    function(timer) 
      function_to_timeout();
    end
  );
  timer:start();
end

function build_uri(url, params)
  if params == nil then return url; end
  
  local params_string = "";
  for k, v in pairs(params) do
    params_string = params_string..k.."="..v.."&";
  end
  
  params_string = strsub(params_string, 1, strlen(params_string) - 1);
  if not params_string then
    return url.."?"..params_string;
  end
  
  return url;
end

function send_request(url, method, headers, body, success_callback, failed_callback, error_callback)
  http.request(
    url, method, headers, body, 
    function(code, data)
      if code < 0 then
        if error_callback == nil then
          print("REQUEST FATAL ERROR: "..code);
        else
          set_timeout(function() error_callback(code); end, REQUEST_ERROR_DELAY);
        end
      else
        print(data);
        local data_obj = sjson.decode(data);
        if data_obj.error ~= sjson.NULL then
          if failed_callback == nil then
            if data_obj.error ~= nil and data_obj.error.error_message ~= nil then
              print("REQUEST ERROR: "..data_obj.error.error_message)
            end
          else
            failed_callback(data_obj.error);
          end
        else
          success_callback(data_obj.data);
        end
      end
    end
  );
end

function send_get(url, params, success_callback, failed_callback, repeat_on_error)
  error_callback = function() 
    print("repeating GET");
    send_get(url, params, success_callback, failed_callback, repeat_on_error);    
  end;
  if repeat_on_error then error_callback = nil end;

  local uri = build_uri(url, parameters);
  send_request(uri, "GET", nil, nil, success_callback, failed_callback, error_callback);
end

function send_post(url, content_type, body, success_callback, failed_callback, repeat_on_error)
  error_callback = function() 
    print("repeating POST");
    send_post(url, content_type, body, success_callback, failed_callback, repeat_on_error);
  end;
  if repeat_on_error then error_callback = nil end;

  send_request(url, "POST", content_type, body, success_callback, failed_callback, error_callback);
end

function send_post_json(url, body, success_callback, failed_callback, repeat_on_error)
  error_callback = function() 
    send_post_json(url, body, success_callback, failed_callback, repeat_on_error);
  end;
  if repeat_on_error then error_callback = nil end;
  
  send_post(url, APP_JSON_CONTENT_TYPE, body, success_callback, failed_callback, error_callback);
end

function get_pin_config(callback)
  send_post_json(
    SERVER_URL.."iot/getconfig", "{ \"mac\" : \""..MAC.."\" }",
    function(data)
      print("config retrieved");
      
      for i, indicator_pin in pairs(data.indicators) do
        INDICATORS[i] = indicator_pin;
        print("INDICATOR: "..indicator_pin);
        gpio.mode(indicator_pin, gpio.OUTPUT);
      end
      
      for i, reader_pin in pairs(data.readers) do
        READERS[i] = { pin = reader_pin, state = 0 };
        READERS_OLD[i] = { pin = reader_pin, state = 0 };
        READERS_ROLLBACK[i] = { pin = reader_pin, state = 0 };
        
        print("READER: "..reader_pin);
        gpio.mode(reader_pin, gpio.INPUT);
        --gpio.mode(reader_pin, gpio.INT)
      end
      
      callback();
    end, nil, true
  );
end

function ping_server()
  send_post_json(
    SERVER_URL.."iot/ping", "{ \"mac\" : \""..MAC.."\" }", 
    function(data)
      for _, pin in pairs(INDICATORS) do
        gpio.write(pin, gpio.LOW);
      end

      if data.enabled_indicator_pins ~= nil then
        for _, pin in pairs(data.enabled_indicator_pins) do
          gpio.write(pin, gpio.HIGH);
        end
      end
    end, nil, false
  );
end

function read_pins()
  print("reading pins");
  for i, reader in pairs(READERS) do
    gpio.mode(reader.pin, gpio.OUTPUT);
    gpio.write(reader.pin, gpio.LOW);

    gpio.mode(reader.pin, gpio.INPUT);
    state = gpio.read(reader.pin);

    READERS_OLD[i].state = READERS[i].state;
    READERS[i].state = state;

    --print("READER: "..reader.pin.." "..state);
  end
end

function get_status_changes()
  changes = {};
  for i, reader in pairs(READERS) do
    changes[i] = { 
      pin = reader.pin, 
      change = READERS[i].state - READERS_OLD[i].state
    };
  end
  return changes;
end

function commit()
  for i, reader in pairs(READERS) do
    READERS_ROLLBACK[i].state = READERS_OLD[i].state;-- READERS[i].state???
  end
end

function rollback()
  for i, reader in pairs(READERS_ROLLBACK) do
    READERS[i].state = READERS_ROLLBACK[i].state;
    READERS_OLD[i].state = READERS_ROLLBACK[i].state;
  end
end

function send_pins()
  print("step probe");
  local status_changes = get_status_changes();
  local step_probe_dto = { mac = MAC, pin_state_changes = status_changes };
  local step_probe_dto_json = sjson.encode(step_probe_dto);
  print(step_probe_dto_json);
  
  send_post_json(
    SERVER_URL.."iot/handlestepprobe", step_probe_dto_json, 
    function(data)
      local status = data["status"];
      if status ~= nil then
        print("STEP PROBE RESULT: "..status);--sjson.encode(data)
        if status == STATUS_DONE or status == STATUS_PENDING then
          commit();
        elseif status == STATUS_ERROR then
          rollback();
        elseif status == STATUS_FINISHED then
          print("finished going deep sleep");
          node.dsleep(0, nil, nil);
        end
      end
    end, nil, false
  );
end

function init_ping_loop()
  set_interval(ping_server, SERVER_PING_INTERVAL);
end

function init_read_loop()
  set_interval(
    function() 
      read_pins(); 
      send_pins(); 
    end, 
    READ_PINS_INTERVAL
  );
end

function build_start()
  get_pin_config(
    function()
      init_ping_loop();
      init_read_loop();
    end
  );
end

print("start");
connect_access_point(
  "Vovan1", "28050528",
  function(connection_info)
    print("CONNECTED, IP: "..connection_info.IP..", MASK: "..connection_info.netmask);
    build_start()
  end,
  function(status)
    print("CONNECTION FAILED: "..status);
    node.restart();
  end
)
