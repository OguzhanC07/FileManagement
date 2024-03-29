import React, { useState, useEffect, useContext } from "react";
import Loader from "react-loader-spinner";
import { useTranslation } from "react-i18next";

import { getfolders } from "../services/folderService";
import FolderTable from "./FolderTable";
import "../styles/table.css";
import {
  FolderContext,
  SETFOLDERS,
  ADDTOFOLDERSTACK,
  REMOVEFROMFOLDERSTACK,
} from "../context/FolderContext";
import BreadcrumbComponent from "./BreadcrumbComponent";
import { FileContext, REMOVEFILES, SETFILES } from "../context/FileContext";
import { getfiles } from "../services/fileService";
import UploadFile from "./UploadFile";

const FetchingData = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const { folder, dispatch } = useContext(FolderContext);
  const { file, dispatch: fileDispatch } = useContext(FileContext);
  const { t } = useTranslation();

  useEffect(() => {
    setIsLoading(true);
    getfolders(folder.folderId)
      .then((res) => {
        dispatch({
          type: SETFOLDERS,
          folders: res.data.data,
        });
      })
      .then(() => {
        if (folder.folderId !== 0) {
          getfiles(folder.folderId).then((res) => {
            if (res.status < 400) {
              fileDispatch({
                type: SETFILES,
                files: res.data.data,
              });
            }
            setIsLoading(false);
          });
        }
        setIsLoading(false);
      })
      .catch((err) => {
        setError(err.message);
        setIsLoading(false);
      });
  }, [dispatch, fileDispatch, folder.folderId]);

  if (error) {
    return (
      <p>
        {t("fetchingData.fetchDataError")} {t("responseErrors.connection")}
      </p>
    );
  }

  const setFolderHandler = (id, name) => {
    fileDispatch({
      type: REMOVEFILES,
      files: [],
    });
    dispatch({
      type: ADDTOFOLDERSTACK,
      folderInfo: {
        id,
        folderName: name,
      },
    });
  };

  const removeStackHandler = (id) => {
    fileDispatch({
      type: REMOVEFILES,
      files: [],
    });
    dispatch({
      type: REMOVEFROMFOLDERSTACK,
      fid: id,
    });
  };

  if (isLoading) {
    return (
      <Loader
        type="Puff"
        color="#00BFFF"
        height={100}
        width={100}
        visible={isLoading}
      />
    );
  }

  return (
    <div className="table">
      {folder.folders.length === 0 && folder.folderId === 0 ? null : (
        <UploadFile />
      )}
      <BreadcrumbComponent
        removeStack={removeStackHandler}
        folderStack={folder.folderStack}
      />
      {folder.folders.length === 0 &&
      file.files.length === 0 &&
      !isLoading &&
      !error ? (
        <p>{t("fetchingData.emptyFolderMessage")}</p>
      ) : (
        <FolderTable
          openFolder={(id, name) => {
            setFolderHandler(id, name);
          }}
        />
      )}
    </div>
  );
};

export default FetchingData;
