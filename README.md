# Comp
assembler for lpnu DeComp
made using C# and win forms

How to use:
dvar - decimal variable
bvar - binary variable
instructions are from DeComp
Lab - represents next after it instruction to jump

to run as user : C:\Users\User_pc\DIY\C#\Comp\Comp\Comp\bin\Debug\Comp.exe

for sourse code open vs solution

![image](https://github.com/user-attachments/assets/b82f30bb-70e3-4b8c-9576-5beda2474af3)
example code (lab 4)
```
bvar A = 0000000000101100        ; Вхідне число
dvar result = 0  ; Лічильник пар "00"
dvar mask = 3    ; Маска 0b11 (для перевірки останніх двох біт)
dvar ONE = 1        ; Константа 1
dvar BIT_COUNT = 8  ; Лічильник ітерацій що залишились
dvar EIGHT = 8      ; Лічильник 8 для 8 біт

Lab Loop :
    LOAD BIT_COUNT  ; Завантажити кількість залишкових ітерацій
    SUB ONE         ; Зменшити на 1
    STORE BIT_COUNT ; Оновити кількість ітерацій
    JM Invert       ; Якщо ітерації закінчилися, перейти до інвертування

    LOAD A
    AND mask         ; Виділити два молодших біти числа
    JZ case00        ; Якщо "00" (останні два біти 0), перейти до обробки випадку "00"
    SUB ONE
    JZ caseElse      ; Якщо "01" або "10", перейти до випадку "else"
    SUB ONE
    JZ caseElse      ; Якщо "11", перейти до випадку "case11"
    
Lab case11 :
    LOAD result     ; Завантажити значення результату
    LSL             ; Зсунути результат на 2 біти вліво
    LSL             ; Зсунути ще на 2 біти
    STORE result    ; Зберегти результат

    LOAD A
    LSR             ; Зсунути праворуч на 2 біти
    LSR             ; Щоб перейти до наступної пари біт
    STORE A         ; Зберегти змінене значення A
    JMP Loop        ; Повернутися до головного циклу
    
Lab case00 :       ; Якщо пара біт "00"
Lab caseElse :     ; Для інших випадків
    LOAD result
    LSL             ; Зсунути результат на 2 біти вліво
    LSL             ; Зсунути ще на 2 біти
    STORE result    ; Зберегти результат

    LOAD A
    AND mask        ; Виділити наступну пару біт
    OR result       ; Додати результат до A
    STORE result    ; Зберегти результат

    LOAD A
    LSR             ; Зсунути праворуч на 2 біти
    LSR             ; Щоб перейти до наступної пари біт
    STORE A         ; Зберегти змінене значення A
    JMP Loop        ; Повернутися до головного циклу

Lab Invert :       ; Мітка для інвертування
    LOAD EIGHT      ; Завантажити значення 8 для 8 біт
    STORE BIT_COUNT ; Ініціалізувати лічильник ітерацій

Lab InvertLoop :
    LOAD BIT_COUNT  ; Завантажити кількість залишкових ітерацій
    SUB ONE         ; Зменшити на 1
    STORE BIT_COUNT ; Оновити кількість ітерацій
    JM End          ; Якщо ітерації завершено, перейти до кінця

    LOAD A
    LSL             ; Зсунути A на 2 біти вліво
    LSL             ; Зсунути ще на 2 біти
    STORE A         ; Зберегти змінене A

    LOAD result
    AND mask        ; Виділити два молодших біти з результату
    OR A            ; Об'єднати з A
    STORE A         ; Зберегти результат

    LOAD result
    LSR             ; Зсунути праворуч на 2 біти
    LSR             ; Перейти до наступної пари біт
    STORE result    ; Зберегти результат
    JMP InvertLoop  ; Повернутися до циклу інвертування
    
Lab End :
    LOAD A
    STORE result    ; Зберегти результат в кінцеву змінну
    HALT            ; Завершити програму

```
