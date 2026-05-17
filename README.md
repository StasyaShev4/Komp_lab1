# Анализ и преобразование кода с использованием Clang и LLVM

## Сведения об авторе
Работа выполнена: Шевяковой Анастасией Игоревной
Группа студента: АВТ-314

## Цель работы

Познакомиться с инструментарием Clang и LLVM, освоить получение абстрактного синтаксического дерева (AST) и промежуточного представления (LLVM IR) для кода на C/C++, научиться применять базовые оптимизации, строить графы потока управления (CFG), а также анализировать влияние оптимизаций на различные синтаксические конструкции языка.

---
## Постановка задачи

Необходимо выполнить следующие шаги:

1. **Установка среды**  
    Установить Clang, LLVM, opt и Graphviz (например, в Ubuntu 26.04).
    
2. **Работа с AST**  
    Сгенерировать абстрактное синтаксическое дерево для заданного C/C++‑файла.
    
3. **Генерация LLVM IR**  
    Получить промежуточное представление кода без оптимизаций (-O0) и с оптимизациями (-O2).
    
4. **Оптимизация IR**  
    Применить оптимизации с помощью opt и/или флагов Clang, сравнить изменения.
    
5. **Построение CFG**  
    Построить граф потока управления для одной или нескольких функций.
    
6. **Индивидуальное задание (по варианту)**  
    Выполнить анализ конкретной синтаксической конструкции в соответствии с вариантом. Сформулировать, как LLVM обрабатывает выбранную конструкцию, какие оптимизации применяются.
    **Вариант:** Структуры / записи
    `Задания:` 
    1. `Получите AST и LLVM IR.` 
    2. `Примените -O2 и опишите, изменилась ли передача структуры (по значению / по ссылке?).` 
    3. `Постройте CFG для sum и main.` 
    4. `Исследуйте, что происходит, если добавить __attribute__((always_inline)) к sum.` 
    5. `Вывод: как LLVM оптимизирует передачу структур?`
    
7. **Выводы**  
    Ответить на контрольные вопросы


---
## Общее задание:
Для начало был создан файл main.c с содержанием: 
```c
#include <stdio.h>
int square(int x) {
 return x * x;
}

int main() {
 int a = 5;
 int b = square(a);
 printf("%d\n", b);
 return 0;
}
```
### 1. Работа с AST
Для исходного файла программы было сгенерировано абстрактное синтаксическое дерево (AST) с помощью команды:
```bash  
clang -Xclang -ast-dump -fsyntax-only main.c 
```

```
FunctionDecl 0x3ca4dd90 <main.c:3:1, line:5:1> line:3:5 used square 'int (int)'  
|-ParmVarDecl 0x3ca4dcf8 <col:12, col:16> col:16 used x 'int'  
`-CompoundStmt  
`-ReturnStmt  
`-BinaryOperator 'int' '*'
```

### 2. Генерация LLVM IR 
LLVM IR без оптимизаций был получен командой:
```bash 
clang -S -emit-llvm -O0 main.c -o main_O0.ll
```
Содержимое:
```
; ModuleID = 'main.c'
source_filename = "main.c"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-linux-gnu"

@.str = private unnamed_addr constant [4 x i8] c"%d\0A\00", align 1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @square(i32 noundef %0) #0 {
  %2 = alloca i32, align 4
  store i32 %0, i32* %2, align 4
  %3 = load i32, i32* %2, align 4
  %4 = load i32, i32* %2, align 4
  %5 = mul nsw i32 %3, %4
  ret i32 %5
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @main() #0 {
  %1 = alloca i32, align 4
  %2 = alloca i32, align 4
  %3 = alloca i32, align 4
  store i32 0, i32* %1, align 4
  store i32 5, i32* %2, align 4
  %4 = load i32, i32* %2, align 4
  %5 = call i32 @square(i32 noundef %4)
  store i32 %5, i32* %3, align 4
  %6 = load i32, i32* %3, align 4
  %7 = call i32 (i8*, ...) @printf(i8* noundef getelementptr inbounds ([4 x i8], [4 x i8]* @.str, i64 0, i64 0), i32 noundef %6)
  ret i32 0
}

declare i32 @printf(i8* noundef, ...) #1

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { "frame-pointer"="all" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.module.flags = !{!0, !1, !2, !3, !4}
!llvm.ident = !{!5}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"PIE Level", i32 2}
!3 = !{i32 7, !"uwtable", i32 1}
!4 = !{i32 7, !"frame-pointer", i32 2}
!5 = !{!"Ubuntu clang version 14.0.0-1ubuntu1.1"}

```

### 3. Оптимизация IR
LLVM IR с оптимизациями:
```bash 
clang -S -emit-llvm -O2 main.c -o main_O2.ll
```
Содержимое:
```
; ModuleID = 'main.c'
source_filename = "main.c"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-linux-gnu"

@.str = private unnamed_addr constant [4 x i8] c"%d\0A\00", align 1

; Function Attrs: mustprogress nofree norecurse nosync nounwind readnone uwtable willreturn
define dso_local i32 @square(i32 noundef %0) local_unnamed_addr #0 {
  %2 = mul nsw i32 %0, %0
  ret i32 %2
}

; Function Attrs: nofree nounwind uwtable
define dso_local i32 @main() local_unnamed_addr #1 {
  %1 = tail call i32 (i8*, ...) @printf(i8* noundef nonnull dereferenceable(1) getelementptr inbounds ([4 x i8], [4 x i8]* @.str, i64 0, i64 0), i32 noundef 25)
  ret i32 0
}

; Function Attrs: nofree nounwind
declare noundef i32 @printf(i8* nocapture noundef readonly, ...) local_unnamed_addr #2

attributes #0 = { mustprogress nofree norecurse nosync nounwind readnone uwtable willreturn "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nofree nounwind uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #2 = { nofree nounwind "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.module.flags = !{!0, !1, !2, !3}
!llvm.ident = !{!4}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"PIE Level", i32 2}
!3 = !{i32 7, !"uwtable", i32 1}
!4 = !{!"Ubuntu clang version 14.0.0-1ubuntu1.1"}
```

Для анализа оптимизаций использовались возможности LLVM и флаг `-O2`.

После оптимизации:
- функция `square` была встроена;
- часть вычислений выполнена на этапе компиляции;
- код был переведён в SSA-форму.

Сравнение двух файлов было проведено командой:
```bash
diff main_O0.ll main_O2.ll
```
Результат: 
```
aboba226@aboba226-VirtualBox:~$ diff main_O0.ll main_O2.ll
8,15c8,11
< ; Function Attrs: noinline nounwind optnone uwtable
< define dso_local i32 @square(i32 noundef %0) #0 {
<   %2 = alloca i32, align 4
<   store i32 %0, i32* %2, align 4
<   %3 = load i32, i32* %2, align 4
<   %4 = load i32, i32* %2, align 4
<   %5 = mul nsw i32 %3, %4
<   ret i32 %5
---
> ; Function Attrs: mustprogress nofree norecurse nosync nounwind readnone uwtable willreturn
> define dso_local i32 @square(i32 noundef %0) local_unnamed_addr #0 {
>   %2 = mul nsw i32 %0, %0
>   ret i32 %2
18,29c14,16
< ; Function Attrs: noinline nounwind optnone uwtable
< define dso_local i32 @main() #0 {
<   %1 = alloca i32, align 4
<   %2 = alloca i32, align 4
<   %3 = alloca i32, align 4
<   store i32 0, i32* %1, align 4
<   store i32 5, i32* %2, align 4
<   %4 = load i32, i32* %2, align 4
<   %5 = call i32 @square(i32 noundef %4)
<   store i32 %5, i32* %3, align 4
<   %6 = load i32, i32* %3, align 4
<   %7 = call i32 (i8*, ...) @printf(i8* noundef getelementptr inbounds ([4 x i8], [4 x i8]* @.str, i64 0, i64 0), i32 noundef %6)
---
> ; Function Attrs: nofree nounwind uwtable
> define dso_local i32 @main() local_unnamed_addr #1 {
>   %1 = tail call i32 (i8*, ...) @printf(i8* noundef nonnull dereferenceable(1) getelementptr inbounds ([4 x i8], [4 x i8]* @.str, i64 0, i64 0), i32 noundef 25)
33c20,21
< declare i32 @printf(i8* noundef, ...) #1
---
> ; Function Attrs: nofree nounwind
> declare noundef i32 @printf(i8* nocapture noundef readonly, ...) local_unnamed_addr #2
35,36c23,25
< attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
< attributes #1 = { "frame-pointer"="all" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
---
> attributes #0 = { mustprogress nofree norecurse nosync nounwind readnone uwtable willreturn "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
> attributes #1 = { nofree nounwind uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
> attributes #2 = { nofree nounwind "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
38,39c27,28
< !llvm.module.flags = !{!0, !1, !2, !3, !4}
< !llvm.ident = !{!5}
---
> !llvm.module.flags = !{!0, !1, !2, !3}
> !llvm.ident = !{!4}
45,46c34
< !4 = !{i32 7, !"frame-pointer", i32 2}
< !5 = !{!"Ubuntu clang version 14.0.0-1ubuntu1.1"}
---
> !4 = !{!"Ubuntu clang version 14.0.0-1ubuntu1.1"}

```

При сравнении IR было установлено, что после применения `-O2` уменьшается количество инструкций `alloca`, `load`, `store`, а также упрощается поток управления.
### 4. Построение CFG
Для построения графа потока управления (CFG) использовалась команда:
```bash
bashclang -O2 -S -emit-llvm main.c -o main.llopt -dot-cfg -disable-output main.ll
```
После выполнения команды были сгенерированы файлы:
```
.square.dot
.main.dot
```
Для визуализации графов использовался Graphviz:
```bash
dot -Tpng .main.dot -o cfg_main.png
dot -Tpng .square.dot -o cfg_square.png
```
Результат:
cfg_main.png
![[cfg_main.png]](cfg_main.png)
cfg_square.png
![[cfg_square.png]](cfg_square.png)

---
## Индивидуальное задание:
**Вариант:** Структуры / записи 
```
#include <stdio.h>

struct Point {
    int x;
    int y;
};

int sum(struct Point p) { 
	return p.x + p.y; 
}

int main() {
    struct Point p = {2, 3};
    int result = sum(p);
    printf("%d\n", result);
    return 0;
}
```
### Анализ конкретной синтаксической конструкции
AST
```
RecordDecl struct Point
|-FieldDecl x 'int'
`-FieldDecl y 'int'

FunctionDecl sum 'int (struct Point)'
|-ParmVarDecl p 'struct Point'
`-CompoundStmt
  `-ReturnStmt
    `-BinaryOperator 'int' '+'
      |-MemberExpr .x
      `-MemberExpr .y
```

###  IR для -O0 и -O2
struct_O0.ll
```
; ModuleID = 'struct.c'
source_filename = "struct.c"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-linux-gnu"

%struct.Point = type { i32, i32 }

@__const.main.p = private unnamed_addr constant %struct.Point { i32 2, i32 3 }, align 4
@.str = private unnamed_addr constant [4 x i8] c"%d\0A\00", align 1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @sum(i64 %0) #0 {
  %2 = alloca %struct.Point, align 4
  %3 = bitcast %struct.Point* %2 to i64*
  store i64 %0, i64* %3, align 4
  %4 = getelementptr inbounds %struct.Point, %struct.Point* %2, i32 0, i32 0
  %5 = load i32, i32* %4, align 4
  %6 = getelementptr inbounds %struct.Point, %struct.Point* %2, i32 0, i32 1
  %7 = load i32, i32* %6, align 4
  %8 = add nsw i32 %5, %7
  ret i32 %8
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @main() #0 {
  %1 = alloca i32, align 4
  %2 = alloca %struct.Point, align 4
  %3 = alloca i32, align 4
  store i32 0, i32* %1, align 4
  %4 = bitcast %struct.Point* %2 to i8*
  call void @llvm.memcpy.p0i8.p0i8.i64(i8* align 4 %4, i8* align 4 bitcast (%struct.Point* @__const.main.p to i8*), i64 8, i1 false)
  %5 = bitcast %struct.Point* %2 to i64*
  %6 = load i64, i64* %5, align 4
  %7 = call i32 @sum(i64 %6)
  store i32 %7, i32* %3, align 4
  %8 = load i32, i32* %3, align 4
  %9 = call i32 (i8*, ...) @printf(i8* noundef getelementptr inbounds ([4 x i8], [4 x i8]* @.str, i64 0, i64 0), i32 noundef %8)
  ret i32 0
}

; Function Attrs: argmemonly nofree nounwind willreturn
declare void @llvm.memcpy.p0i8.p0i8.i64(i8* noalias nocapture writeonly, i8* noalias nocapture readonly, i64, i1 immarg) #1

declare i32 @printf(i8* noundef, ...) #2

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { argmemonly nofree nounwind willreturn }
attributes #2 = { "frame-pointer"="all" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.module.flags = !{!0, !1, !2, !3, !4}
!llvm.ident = !{!5}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"PIE Level", i32 2}
!3 = !{i32 7, !"uwtable", i32 1}
!4 = !{i32 7, !"frame-pointer", i32 2}
!5 = !{!"Ubuntu clang version 14.0.0-1ubuntu1.1"}

```

struct_O2.ll
```
; ModuleID = 'struct.c'
source_filename = "struct.c"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-linux-gnu"

@.str = private unnamed_addr constant [4 x i8] c"%d\0A\00", align 1

; Function Attrs: mustprogress nofree norecurse nosync nounwind readnone uwtable willreturn
define dso_local i32 @sum(i64 %0) local_unnamed_addr #0 {
  %2 = trunc i64 %0 to i32
  %3 = lshr i64 %0, 32
  %4 = trunc i64 %3 to i32
  %5 = add nsw i32 %4, %2
  ret i32 %5
}

; Function Attrs: nofree nounwind uwtable
define dso_local i32 @main() local_unnamed_addr #1 {
  %1 = tail call i32 (i8*, ...) @printf(i8* noundef nonnull dereferenceable(1) getelementptr inbounds ([4 x i8], [4 x i8]* @.str, i64 0, i64 0), i32 noundef 5)
  ret i32 0
}

; Function Attrs: nofree nounwind
declare noundef i32 @printf(i8* nocapture noundef readonly, ...) local_unnamed_addr #2

attributes #0 = { mustprogress nofree norecurse nosync nounwind readnone uwtable willreturn "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nofree nounwind uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #2 = { nofree nounwind "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.module.flags = !{!0, !1, !2, !3}
!llvm.ident = !{!4}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"PIE Level", i32 2}
!3 = !{i32 7, !"uwtable", i32 1}
!4 = !{!"Ubuntu clang version 14.0.0-1ubuntu1.1"}
```

В LLVM IR без оптимизаций структура `Point` передаётся через память. Для этого используются инструкции `alloca`, `store`, `load` и `llvm.memcpy`. В функции `sum` структура временно размещается в памяти:

```
%2 = alloca %struct.Point, align 4
store i64 %0, i64* %3, align 4
```

При оптимизации `-O2` LLVM устраняет обращения к памяти и улучшает передачу структуры. Структура передаётся как 64-битное значение (`i64`), а доступ к её полям осуществляется через операции `trunc` и `lshr`:

```
define dso_local i32 @sum(i64 %0)
```

Кроме того, при оптимизации результат вычисления `sum({2,3})` вычисляется на этапе компиляции, и функция `sum` в `main` фактически не вызывается:

```
printf(..., i32 noundef 5)
```

### CFG для sum и main

![[Ssum_cfg.png]](Ssum_cfg.png)

![[Smain_cfg.png]](Smain_cfg.png)

### Исследование `__attribute__((always_inline))`
```
#include <stdio.h>

struct Point {
    int x;
    int y;
};

__attribute__((always_inline)) inline int sum(struct Point p) {
    return p.x + p.y;
}

int main() {
    struct Point p = {2, 3};
    int result = sum(p);
    printf("%d\n", result);
    return 0;
}
```

```bash
clang -S -emit-llvm -O2 struct.c -o inline.ll
```
inline.ll
```
; ModuleID = 'struct.c'
source_filename = "struct.c"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-linux-gnu"

@.str = private unnamed_addr constant [4 x i8] c"%d\0A\00", align 1

; Function Attrs: nofree nounwind uwtable
define dso_local i32 @main() local_unnamed_addr #0 {
  %1 = tail call i32 (i8*, ...) @printf(i8* noundef nonnull dereferenceable(1) getelementptr inbounds ([4 x i8], [4 x i8]* @.str, i64 0, i64 0), i32 noundef 5)
  ret i32 0
}

; Function Attrs: nofree nounwind
declare noundef i32 @printf(i8* nocapture noundef readonly, ...) local_unnamed_addr #1

attributes #0 = { nofree nounwind uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nofree nounwind "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.module.flags = !{!0, !1, !2, !3}
!llvm.ident = !{!4}

!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"PIE Level", i32 2}
!3 = !{i32 7, !"uwtable", i32 1}
!4 = !{!"Ubuntu clang version 14.0.0-1ubuntu1.1"}
```

### Вывод
LLVM эффективно оптимизирует передачу небольших структур.  
В неоптимизированном IR (`-O0`) структура `Point` передаётся через память с использованием инструкций `alloca`, `store`, `load` и `llvm.memcpy`. Это приводит к дополнительным обращениям к памяти и созданию временных копий структуры.
 После применения оптимизации `-O2` LLVM устраняет лишние операции работы с памятью и представляет структуру как одно 64-битное значение (`i64`). Доступ к полям структуры выполняется через побитовые операции `trunc` и `lshr`, что позволяет передавать структуру через регистры процессора вместо памяти. Кроме того, LLVM выполняет оптимизацию вычислений на этапе компиляции (constant folding). В результате выражение `sum({2,3})` было вычислено заранее, а вызов функции `sum` удалён из `main`.
При использовании `__attribute__((always_inline))` функция `sum` принудительно встраивается в место вызова, что дополнительно уменьшает накладные расходы вызова функции и упрощает CFG.
Таким образом, LLVM оптимизирует передачу структур за счёт:

- уменьшения обращений к памяти;
- передачи небольших структур через регистры;
- удаления лишних копирований;
- встраивания функций (`inline`);
- вычисления констант на этапе компиляции.
---
##  Выводы: 
LLVM эффективно оптимизирует небольшие структуры.  
При использовании `-O2` уменьшается количество обращений к памяти, структура передаётся через регистры вместо временных копий в памяти, а лишние инструкции удаляются.  
Также LLVM выполняет встраивание функций (`inline`) и вычисляет константные выражения на этапе компиляции, что упрощает IR и делает код более эффективным.

### Ответы на вопросы:
1. **Что такое Clang, и какова его роль в процессе компиляции**
**программ?**
Clang — это фронтенд компилятора для языков C, C++ и Objective-C. Он выполняет лексический, синтаксический и семантический анализ исходного кода, строит AST и преобразует программу в LLVM IR.

2. **Что представляет собой LLVM и как он используется в**
**современных компиляторах?**
LLVM — это модульная инфраструктура для создания компиляторов. LLVM используется для оптимизации промежуточного представления программы и генерации машинного кода для разных архитектур.

3. **Чем отличается абстрактное синтаксическое дерево (AST) от**
**промежуточного представления LLVM IR?**
AST описывает структуру программы на уровне языка программирования, а LLVM IR представляет программу в низкоуровневой промежуточной форме, удобной для анализа и оптимизации.

4. **Для чего необходимо промежуточное представление (IR) в**
**процессе компиляции?**
Промежуточное представление (IR) необходимо для выполнения оптимизаций и независимой от платформы обработки программы перед генерацией машинного кода.

5. **Что делает инструкция alloc в LLVM IR, и зачем она**
**используется в функциях?**
Инструкция `alloca` выделяет память в стеке функции для локальных переменных. Она используется в неоптимизированном IR для хранения значений в памяти.

6. **Зачем нужна оптимизация кода в компиляторе, и какие**
**основные цели она преследует?**
Оптимизация кода нужна для повышения производительности программы, уменьшения размера кода и сокращения количества лишних операций и обращений к памяти.

7. **Что такое SSA-форма и почему она важна при оптимизации**
**программ?**
SSA-форма — это представление программы, в котором каждая переменная присваивается только один раз. Она упрощает анализ зависимостей и позволяет эффективно выполнять оптимизации.

8. **Что такое граф потока управления (CFG) и как он помогает**
**анализировать поведение программы?**
CFG (граф потока управления) — это граф базовых блоков и переходов между ними. Он используется для анализа поведения программы и выполнения оптимизаций, связанных с потоком управления.

9. **Как устроено представление арифметических операций в**
**LLVM IR (например, умножение, сложение)?**
Арифметические операции в LLVM IR представлены отдельными инструкциями, например:
- `add` — сложение;
- `sub` — вычитание;
- `mul` — умножение.  
Каждая операция работает с явно указанными операндами и типами данных.

10. **Почему функции в LLVM IR обычно представляют собой**
**отдельные единицы анализа и оптимизации?**
Функции в LLVM IR рассматриваются как отдельные единицы анализа и оптимизации, потому что это упрощает анализ зависимостей, построение CFG и применение оптимизаций

11. **Что происходит с функцией в LLVM IR, если она вызывается**
**один раз и очень короткая?**
Если функция короткая и вызывается один раз, LLVM может встроить её (`inline`) непосредственно в место вызова, удалив отдельный вызов функции.

12. **Какие преимущества даёт использование IR и CFG для**
**автоматических оптимизаций по сравнению с анализом исходного текста**
**на C?**
Использование IR и CFG позволяет анализировать программу на более низком и формальном уровне, чем исходный код C. Это упрощает автоматическое выполнение оптимизаций, анализ зависимостей и преобразование программы независимо от синтаксиса языка.

---
## Дополнительное задание
Локальные оптимизации синтаксических конструкций на промежуточном представлении
Для выбранной синтаксической конструкции (в соответствии с вашим вариантом КР / РГР): 
1. Построить абстрактное синтаксическое дерево (AST) конструкции (из лабораторной работы 5). 
2. Сгенерировать промежуточное представление (IR) для данной конструкции в виде трехадресного кода (TAC) или списка виртуальных инструкций или канонической строковой формы (упрощённый вариант для одной строки). 
3. Реализовать две локальные оптимизации. Каждая оптимизация должна преобразовывать IR к эквивалентному, но более простому или каноническому виду. 
4. Продемонстрировать работу оптимизаций на конкретных примерах: показать «входной IR» и «выходной IR» после применения каждой оптимизации.

### AST
Конструкция: 
```
struct User {
    int $id = 2 + 3 * 4;
    int $score = (10 + 5);
    string $name = "Guest";
    bool $active = true;
    int $id1 = 100;
};
```
![[AST 1.png]](AST 1.png)
![[Ast_text.png]](Ast_text.png)

### Промежуточное представление (IR) виде трехадресного кода (TAC)
![[TAC.png]](TAC.png)

### Две локальные оптимизации.
#### 1 - Constant Folding
![[TAC_1.png]](TAC_1.png)
##### Блок-схема Constant Folding
Основана на методе: OptimizeConstantFolding()
![[B_1.png]](B_1.png)
#### 2 - Temp Variable Elimination
![[TAC_2.png]](TAC_2.png)
##### Блок-схема Temp Variable Elimination
Основана на: OptimizeTempVariables()
![[B_2.png]](B_2.png)