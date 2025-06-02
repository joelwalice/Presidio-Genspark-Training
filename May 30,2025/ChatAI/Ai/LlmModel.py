import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.pipeline import Pipeline
from sklearn.metrics import classification_report
from sklearn.naive_bayes import MultinomialNB

class LlmModel:
    def read_csv(self, file_path: str) -> pd.DataFrame:
        try:
            df = pd.read_csv(file_path, encoding='utf-8-sig')
            df.columns = df.columns.str.strip()
            if 'Question' not in df.columns or 'Answer' not in df.columns:
                if df.shape[1] == 3:
                    df.columns = ['Question', 'Answer', 'Class']
                else:
                    raise ValueError("CSV must contain at least 'Question' and 'Answer' columns.")
            df.dropna(subset=['Question'], inplace=True)
            df.drop_duplicates(subset=['Question'], inplace=True)
            return df
        except Exception as e:
            print(f"Error reading CSV file: {e}")
            return pd.DataFrame()

    def train_test_split(self, df: pd.DataFrame, test_size: float = 0.2) -> tuple:
        X_train, X_test, y_train, y_test = train_test_split(df['Question'], df['Answer'], test_size=test_size, random_state=42)
        return X_train, X_test, y_train, y_test

    def Pipeline_creation(self, X_train, y_train):
        model = Pipeline([
            ('tfidf', TfidfVectorizer()),
            ('clf', MultinomialNB())
        ])
        model.fit(X_train, y_train)
        return model

    def Model_evaluation(self):
        X_train, X_test, y_train, y_test = self.train_test_split(df)
        model = self.Pipeline_creation(X_train, y_train)
        y_pred = model.predict(X_test)
        report = classification_report(y_test, y_pred)
        return report

    def analyse_question(self, question: str, model):
        prediction = model.predict([question])[0]
        return prediction

    def Identify_answer(self, question: str, df: pd.DataFrame) -> str:
        for index, row in df.iterrows():
            if question.strip().lower() == row['Question'].strip().lower():
                return row['Answer']
        model = self.Pipeline_creation(df['Question'], df['Answer'])
        prediction = self.analyse_question(question, model)
        return prediction

def main():
    llm = LlmModel()
    df = llm.read_csv('/Users/presidio/Desktop/Presidio-Genspark-Training/May 30,2025/ChatAI/Data/BankFAQs.csv')
    if not df.empty:
        question = "How can I update my email address?"
        answer = llm.Identify_answer(question, df)
        print(f"Answer for the question '{question}': {answer}")
    else:
        print("DataFrame is empty. Please check the CSV file.")

if __name__ == "__main__":
    main()


# import pandas as pd
# import numpy as np
# from sentence_transformers import SentenceTransformer # type: ignore
# from sklearn.metrics.pairwise import cosine_similarity

# class LlmModel:
#     def __init__(self):
#         self.model = SentenceTransformer('all-MiniLM-L6-v2')
#         self.embeddings = None
#         self.questions = None
#         self.answers = None

#     def read_csv(self, file_path: str) -> pd.DataFrame:
#         try:
#             df = pd.read_csv(file_path, encoding='utf-8-sig')
#             df.columns = df.columns.str.strip()
#             df.dropna(subset=['Question'], inplace=True)
#             df.drop_duplicates(subset=['Question'], inplace=True)
#             return df
#         except Exception as e:
#             print(f"Error reading CSV file: {e}")
#             return pd.DataFrame()

#     def encode_questions(self, df: pd.DataFrame):
#         self.questions = df['Question'].tolist()
#         self.answers = df['Answer'].tolist()
#         self.embeddings = self.model.encode(self.questions, convert_to_tensor=False)

#     def find_similar_question(self, input_question: str):
#         input_embedding = self.model.encode([input_question], convert_to_tensor=False)
#         cosine_scores = cosine_similarity(input_embedding, self.embeddings)[0]
#         best_match_idx = np.argmax(cosine_scores)
#         return self.questions[best_match_idx], self.answers[best_match_idx], cosine_scores[best_match_idx]

# def main():
#     llm = LlmModel()
#     df = llm.read_csv("BankFAQs.csv")
#     if not df.empty:
#         llm.encode_questions(df)
#         input_q = "How do I change my email?"
#         matched_q, answer, score = llm.find_similar_question(input_q)
#         print(f"Input: {input_q}")
#         print(f"Matched Question: {matched_q}")
#         print(f"Answer: {answer}")
#         print(f"Confidence: {score:.2f}")
#     else:
#         print("DataFrame is empty.")

# if __name__ == "__main__":
#     main()
