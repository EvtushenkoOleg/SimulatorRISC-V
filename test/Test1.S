.data
N:
.word 5
M:
.word 4
matrix:
.word 7, 4, 5, 5, 7
.word 11, 4, 5, 8, 10
.word 3, 12, 2, 6 ,4
.word 9, 7, 4, 8, 3
result:
.word 0

.text

main:
la a2, N
la a3, M
la a4, matrix
la a5, result
call process
call print
call exit

print:

addi a0, x0, 1
lw a1, 0(a5)
ecall
ret

exit:
addi a0, x0, 10
ecall

process:
lw t1, 0(a2) # N
lw t2, 0(a3) # M

addi t4, x0, 1 # row pointer
addi t3, x0, 0 # row element number
lw t0, 0(a4) # t0-минимальный элемент.
addi a4, a4, 4 # increment array pointer

cycle:
lw t5, 0(a4) # load value from array
blt t0, t5, cont
add t0, x0, t5

cont:
addi a4, a4, 4 # increment array pointer
addi t4, t4, 1
blt t4, t1, cycle
addi t4, x0, 0
addi t3, t3, 1
blt t3, t2, cycle
sw t0, 0(a5)  # save result
ret