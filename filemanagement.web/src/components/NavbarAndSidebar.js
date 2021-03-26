import React, { useState, useContext } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Icon } from "semantic-ui-react";

import LanguageDropDown from "../components/LanguageDropdown";
import "../styles/admin.css";
import { AuthContext, LOGOUT } from "../context/AuthContext";

const NavbarAndSidebar = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [active, setActive] = useState("main");
  const { dispatch } = useContext(AuthContext);
  const { t } = useTranslation();
  const logouthandler = () => {
    dispatch({
      type: LOGOUT,
    });
  };
  return (
    <div>
      <button
        onClick={() => {
          setIsOpen((prevState) => !prevState);
        }}
        className={`btn-nav ${isOpen ? "animated" : ""}`}
      >
        <div className="bar arrow-top-r"></div>
        <div className="bar arrow-middle-r"></div>
        <div className="bar arrow-bottom-r"></div>
      </button>
      <div className="links-con">
        <LanguageDropDown />
        <button
          onClick={() => {
            logouthandler();
          }}
          className="nav"
        >
          <Icon name="log out" />
          {t("topMenu.logOut")}
        </button>
      </div>
      <main>
        <aside className={`left-side ${isOpen ? "showNav" : "hideNav"}`}>
          <ul className="effects-list">
            <li className="item-divider">
              <Icon name="folder" />
              File Management
            </li>
            <li className={`item ${active === "main" ? "item-active" : ""}`}>
              <Link
                onClick={() => {
                  setActive("main");
                  setIsOpen(false);
                }}
                to="/"
              >
                {t("sideMenu.myFiles")}
              </Link>
            </li>
            <li className={`item ${active === "about" ? "item-active" : ""}`}>
              <Link
                to="/about"
                onClick={() => {
                  setActive("about");
                  setIsOpen(false);
                }}
              >
                {t("sideMenu.sharedFiles")}
              </Link>
            </li>
          </ul>
        </aside>
      </main>
    </div>
  );
};

export default NavbarAndSidebar;
