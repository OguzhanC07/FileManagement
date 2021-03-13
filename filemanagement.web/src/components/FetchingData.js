import React, { useState, useEffect, useContext } from "react";
import Loader from "react-loader-spinner";

import { getfolders } from "../services/folderService";
import FolderTable from "./FolderTable";
import "../styles/table.css";
import { FolderContext, SETFOLDERS } from "../context/FolderContext";

const FetchingData = (props) => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const { folder, dispatch } = useContext(FolderContext);

  useEffect(() => {
    setIsLoading(true);
    getfolders()
      .then((res) => {
        dispatch({
          type: SETFOLDERS,
          folders: res.data.data,
        });
      })
      .catch((err) => {
        setError(err.message);
      });
    setIsLoading(false);
  }, [dispatch]);

  if (error) {
    return (
      <p>
        Dosyaları çekerken bir hata oluştu. Lütfen tekrar giriş yapınız. {error}
      </p>
    );
  }

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
      {folder.folders.length === 0 && !error && !isLoading ? (
        <p>Hiç klasör bulunamadı. Yeni klasör eklemek ister misiniz?</p>
      ) : (
        <FolderTable data={folder.folders} />
      )}
    </div>
  );
};

export default FetchingData;
