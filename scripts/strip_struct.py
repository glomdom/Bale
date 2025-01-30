import sys
import re

def remove_comment_lines(c_enum: str) -> str:
    lines = c_enum.split("\n")
    return "\n".join(line for line in lines if not line.strip().startswith("//"))

def convert(c_enum: str, enum_name: str) -> str:
    # Remove comment lines
    c_enum = remove_comment_lines(c_enum)
    
    # Regular expression to capture enum fields and values
    enum_pattern = re.compile(r"\s*(\w+)\s*(=\s*[^,\n]*)?,?")
    
    # Extract the fields from the C enum
    lines = c_enum.strip().split("\n")
    csharp_enum_lines = []
    
    for line in lines:
        line = line.strip()
        if line.startswith("#ifdef"):
            macro = line.split()[1]
            csharp_enum_lines.append(f"#if {macro}")
        elif line.startswith("#ifndef"):
            macro = line.split()[1]
            csharp_enum_lines.append(f"#if !{macro}")
        elif line.startswith("#else"):
            csharp_enum_lines.append("#else")
        elif line.startswith("#endif"):
            csharp_enum_lines.append("#endif")
        else:
            match = enum_pattern.search(line)

            if match:
                field_name = match.group(1)
                field_value = match.group(2) if match.group(2) else ""
                
                csharp_line = f"    {field_name}{' ' + field_value if field_value else ''},"
                csharp_enum_lines.append(csharp_line)
    
    csharp_enum = f"public enum {enum_name} \n{{\n" + "\n".join(csharp_enum_lines) + "\n}"   
    
    return csharp_enum

def main():
    fname = sys.argv[1]

    with open(fname) as fp:
        data = fp.read()

    cleaned = convert(data, "VkStructureType")

    print(cleaned)

if __name__ == "__main__":
    main()