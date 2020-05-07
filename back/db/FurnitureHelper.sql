CREATE TABLE accounts(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    login VARCHAR(64) NOT NULL UNIQUE,
    pwd VARCHAR(256) NOT NULL,
    email VARCHAR(256) NOT NULL UNIQUE,
	first_name CHAR(64) NOT NULL,
	last_name CHAR(64) NOT NULL
);

CREATE TABLE admins(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    FOREIGN KEY(account_id) REFERENCES accounts(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE super_admins(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    admin_account_id INTEGER NOT NULL,
    FOREIGN KEY(admin_account_id) REFERENCES admins(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE accounts_extensions(
    id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    phone VARCHAR(32) NOT NULL,
    address VARCHAR(256) NOT NULL,
    last_used DATE NOT NULL,
    FOREIGN KEY(account_id) REFERENCES accounts(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE colors(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(64) NOT NULL UNIQUE,
    hex CHAR(8) NOT NULL,
	description TEXT
);

CREATE TABLE materials(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(128) NOT NULL UNIQUE,
    texture_url VARCHAR(512) NOT NULL,
	price_coeff FLOAT NOT NULL DEFAULT 1,
	description TEXT
);

CREATE TABLE materials_possible_colors(
	material_id INTEGER NOT NULL,
    color_id INTEGER NOT NULL,
    FOREIGN KEY(material_id) REFERENCES materials(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(color_id) REFERENCES colors(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE parts(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(256) NOT NULL,
    model_url VARCHAR(512) NOT NULL,
    price FLOAT NOT NULL,
	description TEXT
);

CREATE TABLE parts_possible_materials(
  	part_id INTEGER NOT NULL,
    material_id INTEGER NOT NULL,
    FOREIGN KEY(part_id) REFERENCES parts(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(material_id) REFERENCES materials(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE part_controllers_embed_relative_positions(
    id INTEGER PRIMARY KEY AUTO_INCREMENT,
    part_id INTEGER NOT NULL,
    pos_x FLOAT NOT NULL,
    pos_y FLOAT NOT NULL,
	pos_z FLOAT NOT NULL,
	indicator_pin_number INTEGER NOT NULL,
	reader_pin_number INTEGER NOT NULL,
    FOREIGN KEY(part_id) REFERENCES parts(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE furniture_items(
    id INTEGER PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(256) NOT NULL,
	description TEXT
);

CREATE TABLE furniture_item_parts_connections(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    furniture_item_id INTEGER NOT NULL,
	comment_text TEXT,
    FOREIGN KEY(furniture_item_id) REFERENCES furniture_items(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE parts_connection_glues(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
	connection_id INTEGER NOT NULL,
    glue_part_id INTEGER,
	comment_text TEXT,
    FOREIGN KEY(connection_id) REFERENCES furniture_item_parts_connections(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(glue_part_id) REFERENCES parts(id) ON UPDATE CASCADE ON DELETE SET NULL
);

CREATE TABLE two_parts_connection(
    id INTEGER PRIMARY KEY AUTO_INCREMENT,
    global_connection_id INTEGER NOT NULL,
    part_controller_id INTEGER NOT NULL,
    part_controller_other_id INTEGER NOT NULL,
	order_number INTEGER NOT NULL DEFAULT 0,
	nested_global_connection_order_number INTEGER,
	nested_two_parts_connection_order_number INTEGER,
	connect_to_first_if_equal BOOL,
	comment_text TEXT,
    FOREIGN KEY(global_connection_id) REFERENCES furniture_item_parts_connections(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(part_controller_id) REFERENCES part_controllers_embed_relative_positions(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(part_controller_other_id) REFERENCES part_controllers_embed_relative_positions(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE two_parts_connection_glues(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
	two_parts_connection_id INTEGER NOT NULL,
    glue_part_id INTEGER,
	comment_text TEXT,
    FOREIGN KEY(two_parts_connection_id) REFERENCES two_parts_connection(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(glue_part_id) REFERENCES parts(id) ON UPDATE CASCADE ON DELETE SET NULL
);

CREATE TABLE concrete_parts(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    part_id INTEGER NOT NULL,
    material_id INTEGER NOT NULL,
    color_id INTEGER NOT NULL,
    create_date DATE NOT NULL,
	controller_mac CHAR(17) NOT NULL UNIQUE,
    FOREIGN KEY(part_id) REFERENCES parts(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(material_id) REFERENCES materials(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(color_id) REFERENCES colors(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE concrete_controllers(
    id INTEGER PRIMARY KEY AUTO_INCREMENT,
    concrete_part_id INTEGER NOT NULL,
    embed_position_id INTEGER NOT NULL,
    FOREIGN KEY(concrete_part_id) REFERENCES concrete_parts(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(embed_position_id) REFERENCES part_controllers_embed_relative_positions(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE ownings(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    concrete_part_id INTEGER NOT NULL,
    FOREIGN KEY(account_id) REFERENCES accounts(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(concrete_part_id) REFERENCES concrete_parts(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE manufacturer_sells(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    accounts_extension_id INTEGER NOT NULL,
    sell_date DATE NOT NULL,
    FOREIGN KEY(accounts_extension_id) REFERENCES accounts_extensions(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE manufacturer_sell_positions (
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    manufacturer_sell_id INTEGER NOT NULL,
    concrete_part_id INTEGER NOT NULL,
    price FLOAT NOT NULL,
    FOREIGN KEY(manufacturer_sell_id) REFERENCES manufacturer_sells(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(concrete_part_id) REFERENCES concrete_parts(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE user_sells(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    accounts_extension_id INTEGER NOT NULL,
    accounts_extension_other_id INTEGER NOT NULL,
    sell_date DATE NOT NULL,
    FOREIGN KEY(accounts_extension_id) REFERENCES accounts_extensions(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(accounts_extension_other_id) REFERENCES accounts_extensions(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE user_sell_positions(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    user_sell_id INTEGER NOT NULL,
    concrete_part_id INTEGER NOT NULL,
    price FLOAT NOT NULL,
    FOREIGN KEY(user_sell_id) REFERENCES user_sells(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(concrete_part_id) REFERENCES concrete_parts(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE furniture_item_instructions(
    id INTEGER PRIMARY KEY AUTO_INCREMENT,
    global_connection_id INTEGER NOT NULL,
    global_connection_order_number INTEGER NOT NULL DEFAULT 0,
    step_text TEXT,
    FOREIGN KEY(global_connection_id) REFERENCES furniture_item_parts_connections(id) ON UPDATE CASCADE ON DELETE CASCADE
);