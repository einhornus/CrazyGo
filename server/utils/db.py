import sqlite3

import os
dir = os.path.dirname(__file__)
db_path = dir+'/database'

def execute_query(query):
    assert query != None
    assert isinstance(query, str)

    result = None
    conn = sqlite3.connect(db_path)
    c = conn.cursor()
    c.execute(query)
    conn.commit()
    conn.close()

    return c.lastrowid


def execute_query_fetched(query):
    assert query != None
    assert isinstance(query, str)

    result = None
    print(db_path)
    conn = sqlite3.connect(db_path)
    c = conn.cursor()
    c.execute(query)
    result = c.fetchone()
    conn.close()

    return result


def execute_query_fetched_all(query):
    assert query != None
    assert isinstance(query, str)

    result = None
    conn = sqlite3.connect(db_path)
    c = conn.cursor()
    c.execute(query)
    result = c.fetchall()
    conn.close()

    return result