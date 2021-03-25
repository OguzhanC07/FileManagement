import i18n from "i18next";
import { initReactI18next } from "react-i18next";

import XHR from "i18next-xhr-backend";
import LanguageDetector from "i18next-browser-languagedetector";

import translationEN from "./locales/en/translation.json";
import translationTR from "./locales/tr/translation.json";

i18n
  .use(XHR)
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    debug: false,
    lng: "en",
    fallbackLng: "en",

    // keySeparator: false,

    interpolation: {
      escapeValue: false,
    },

    resources: {
      en: {
        translations: translationEN,
      },
      tr: {
        translations: translationTR,
      },
    },

    ns: ["translations"],
    defaultNS: "translations",
  });

export default i18n;
