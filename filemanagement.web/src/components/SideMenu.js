import React, { useState } from "react";
import { Menu, Icon } from "semantic-ui-react";
// import { Link } from "react-router-dom";
import "semantic-ui-css/semantic.min.css";

import "../styles/index.css";

const SideMenu = (props) => {
  const [activeItem, setActiveItem] = useState("home");
  const handleItemClick = (name) => {
    setActiveItem(name);
  };
  const getMenu = () => {
    return (
      <Menu fixed="left" borderless className="side" vertical>
        <Menu.Item
          as={"a"}
          to={"/"}
          name="home"
          active={activeItem === "home"}
          onClick={() => {
            handleItemClick("home");
          }}
        >
          <Icon name="home" />
          Home
        </Menu.Item>
        <Menu.Item
          as={"a"}
          to={"/folder"}
          name="folder2"
          active={activeItem === "folder"}
          onClick={() => {
            handleItemClick("folder");
          }}
        >
          <Icon name="folder" />
          Route2
        </Menu.Item>
      </Menu>
    );
  };

  return (
    <div className="parent">
      <div className="side">{getMenu()}</div>
      <div className="content">{props.children}</div>
    </div>
  );
};

export default SideMenu;