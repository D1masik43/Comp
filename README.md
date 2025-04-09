# Comp
асемблер для lpnu DeComp
зроблений за допомогою C# і win forms


## Як використовувати:
- ви можете створювати десяткові та двійкові змінні за допомогою dvar і bvar і використовувати їх з інструкціями, які потребують чисел як операндів
- ви можете використовувати назву лабораторії: щоб зробити "перехід до" точки та використовувати мітки з інструкціями jmp
- після компіляції ви можете написати код у DeComp за допомогою кнопки «записати у перфострічку» (потім укажіть шлях до DeComp.prg)

![image](https://github.com/user-attachments/assets/f0d30979-bfcd-4055-895d-ef486a31c2a7)

Код для прикладу (Лаба 3)
                 dvar NUM =  14  ; Вхідне число
dvar RESULT = 0     ; Лічильник нулів
dvar ONE = 1        ; Константа 1
dvar BIT_COUNT = 16 ; Лічильник ітерацій що залишились

Lab MAIN :

LOAD BIT_COUNT
SUB ONE
STORE BIT_COUNT
JM END

LOAD NUM
LSL
JC SHIFT

LOAD RESULT
ADD ONE
STORE RESULT

Lab SHIFT :

LOAD NUM
LSL
STORE NUM
JMP MAIN

Lab END :

