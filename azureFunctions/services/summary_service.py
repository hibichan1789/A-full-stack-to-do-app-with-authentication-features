import os
from openai import OpenAI
import logging

client = OpenAI(api_key=os.environ["OPENAI_API_KEY"])

# あとでsummarize関数をopenAI連携取れるように改良する
def summarize(description:str)->str:
    if description.strip() == "":
        return ""
    
    try:
        response = client.chat.completions.create(
            model="gpt-4o-mini",
            messages=[
                {"role": "system",
                "content":
                "以下の文章から重要なキーワードを3〜5個抽出したリストを作成してください\n\n"
                "出力例:\n"
                "キーワード: A, B, C"},
                {"role": "user", "content": description}
            ],
            max_tokens=120,
            temperature=0.3
        )

        summary = response.choices[0].message.content
        if summary is None:
            return ""
    
        return summary.strip()
    except Exception as e:
        logging.error(f"OpenAI summarize error: {e}")
        return ""