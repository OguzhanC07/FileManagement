import React from "react";
import SideMenu from "./SideMenu";
import TopMenu from "./TopMenu";
import "../styles/index.css";
import "semantic-ui-css/semantic.min.css";

const Layout = (props) => {
  return (
    <div className="grid">
      <div className="menu">
        <TopMenu />
      </div>
      <div className="main-content">
        <SideMenu>{props.children}</SideMenu>
      </div>
    </div>
  );
};

export default Layout;
