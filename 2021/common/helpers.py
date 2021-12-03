def convert_list_to_int(input: list) -> list :
    return list(map(int, input))


def hamming_weight(input: str) -> int :
    one_count = 0
    for i in input:
        if i == '1':
            one_count += 1
    return one_count

def transpose(input: list[str]) -> list[str]:
    input = list(map(list, input))
    new_list = list(zip(*input))
    join = map(join_tuple_string, new_list)
    return list(join)

def join_tuple_string(strings_tuple) -> str:
   return ''.join(strings_tuple)