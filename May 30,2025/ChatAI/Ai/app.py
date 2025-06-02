from flask import Flask, request, jsonify
from LlmModel import LlmModel
import threading

app = Flask(__name__)

# Load model and data once at startup
data_path = '/Users/presidio/Desktop/Presidio-Genspark-Training/May 30,2025/ChatAI/Data/BankFAQs.csv'
llm = LlmModel()
df = llm.read_csv(data_path)

data_lock = threading.Lock()

@app.route('/ask', methods=['POST'])
def ask_question():
    data = request.get_json()
    question = data.get('question', '').strip()
    if not question:
        return jsonify({'error': 'No question provided.'}), 400
    with data_lock:
        answer = llm.Identify_answer(question, df)
    return jsonify({'Question': question, 'Answer': answer})

@app.route('/health', methods=['GET'])
def health():
    return jsonify({'status': 'ok'})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
