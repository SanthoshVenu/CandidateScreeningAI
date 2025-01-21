import spacy

# Load the English NLP model
nlp = spacy.load("en_core_web_sm")

def analyze_response(response_text):
    doc = nlp(response_text)

    # Extract specific information
    salary = None
    relocate = None
    skills = []

    # Example: Identify entities
    for ent in doc.ents:
        if ent.label_ == "MONEY":
            salary = ent.text

    # Example: Match keywords for relocation and skills
    relocate_keywords = ["relocate", "move", "any location"]
    skill_keywords = ["python", "java", "machine learning"]

    if any(word in response_text.lower() for word in relocate_keywords):
        relocate = True
    else:
        relocate = False

    for token in doc:
        if token.text.lower() in skill_keywords:
            skills.append(token.text)

    return {
        "salary": salary,
        "willing_to_relocate": relocate,
        "skills": list(set(skills))  # Remove duplicates
    }
