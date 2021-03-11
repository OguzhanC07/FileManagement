import React, { useContext } from "react";
import { Menu, Image, Icon } from "semantic-ui-react";
import "semantic-ui-css/semantic.min.css";

import icon from "../assets/icon.png";
import "../styles/index.css";

import { AuthContext, LOGOUT } from "../context/AuthContext";

const TopMenu = (props) => {
  const { dispatch } = useContext(AuthContext);

  const logouthandler = () => {
    dispatch({
      type: LOGOUT,
    });
  };

  return (
    <Menu fixed="top" className="top-menu">
      <Menu.Item className="logo-space-menu-item">
        <div className="display-inline logo-space">
          <Image src={icon} />
          <p>File Management</p>
        </div>
      </Menu.Item>
      <Menu.Menu position="right">
        <Menu.Item
          className="no-border"
          onClick={() => {
            logouthandler();
          }}
          position="right"
        >
          <div className="display-inline">
            <Icon name="log out" />
            Log Out
          </div>
        </Menu.Item>
      </Menu.Menu>
    </Menu>
  );
};

export default TopMenu;
