import React from "react";
import { useTranslation } from "react-i18next";

import "../styles/dropdown.css";

const LanguageDropDown = () => {
  const { t, i18n } = useTranslation();
  const selectHandler = (e) => {
    i18n.changeLanguage(e.target.value);
  };
  var language = localStorage.getItem("i18nextLng");

  return (
    <span className="nav custom-dropdown">
      <select
        defaultValue={language !== "en" || "tr" ? "en" : language}
        onChange={(e) => {
          selectHandler(e);
        }}
      >
        <option value="tr">{t("lang.tr")}</option>
        <option value="en">{t("lang.en")}</option>
      </select>
    </span>
  );
};

export default LanguageDropDown;
