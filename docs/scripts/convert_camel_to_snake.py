import re
import os

CAMEL_KEY_RE = re.compile(r'"([a-zA-Z][a-zA-Z0-9]*[A-Z][a-zA-Z0-9]*)"(?=\s*:)')


def to_snake(name):
    s1 = re.sub('(.)([A-Z][a-z]+)', r"\1_\2", name)
    s2 = re.sub('([a-z0-9])([A-Z])', r"\1_\2", s1)
    return s2.lower()


def convert_in_codeblock(text):
    def repl(m):
        key = m.group(1)
        return '"' + to_snake(key) + '"'
    return CAMEL_KEY_RE.sub(repl, text)


def process_file(path):
    with open(path, 'r', encoding='utf-8', errors='replace') as f:
        content = f.read()

    # Replace keys only inside fenced code blocks (``` ... ```)
    def codeblock_repl(m):
        header = m.group(1) or ''
        body = m.group(2) or ''
        converted = convert_in_codeblock(body)
        return '```' + (header or '') + converted + '```'

    new_content = re.sub(r'```([^\n]*\n)?(.*?)```', codeblock_repl, content, flags=re.DOTALL)

    if new_content != content:
        with open(path, 'w', encoding='utf-8') as f:
            f.write(new_content)
        return True
    return False


def main():
    base = os.path.join(os.path.dirname(__file__), '..')
    api_dir = os.path.join(base, 'api')
    changed = []
    for root, dirs, files in os.walk(api_dir):
        for fn in files:
            if fn.endswith('.md'):
                path = os.path.join(root, fn)
                if process_file(path):
                    changed.append(path)
    print('Modified files:', len(changed))
    for p in changed:
        print(p)


if __name__ == '__main__':
    main()
