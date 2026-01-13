import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateFlwGradeHorarios, useUpdateFlwGradeHorarios } from "@/lib/abp/hooks/useFlwGradeHorarioses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idGrade: z.any(),
  
    idHistoricoTipo: z.any(),
  
    manhaHorarioInicial: z.any(),
  
    manhaIntervalo: z.any(),
  
    manhaQtde: z.any(),
  
    tardeHorarioInicial: z.any(),
  
    tardeIntervalo: z.any(),
  
    tardeQtde: z.any(),
  
    dom: z.any(),
  
    seg: z.any(),
  
    ter: z.any(),
  
    qua: z.any(),
  
    qui: z.any(),
  
    sex: z.any(),
  
    sab: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FlwGradeHorariosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FlwGradeHorariosForm({
  isOpen,
  onClose,
  initialValues,
}: FlwGradeHorariosFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateFlwGradeHorarios();
const updateMutation = useUpdateFlwGradeHorarios();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("flwGradeHorarios updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("flwGradeHorarios created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save flwgradehorarios:", error);
    toast.error(error.message || "Failed to save flwgradehorarios");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit flwGradeHorarios": "Create flwGradeHorarios" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the flwgradehorarios." : "Fill in the details to create a new flwgradehorarios." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idGrade" > idGrade</Label>

<Input id="idGrade" type = "number" step = "any" {...register("idGrade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idHistoricoTipo" > idHistoricoTipo * </Label>

<Input id="idHistoricoTipo" type = "number" step = "any" {...register("idHistoricoTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="manhaHorarioInicial" > manhaHorarioInicial</Label>

<Input id="manhaHorarioInicial" type = "number" step = "any" {...register("manhaHorarioInicial") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="manhaIntervalo" > manhaIntervalo</Label>

<Input id="manhaIntervalo" type = "number" step = "any" {...register("manhaIntervalo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="manhaQtde" > manhaQtde</Label>

<Input id="manhaQtde" type = "number" step = "any" {...register("manhaQtde") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tardeHorarioInicial" > tardeHorarioInicial</Label>

<Input id="tardeHorarioInicial" type = "number" step = "any" {...register("tardeHorarioInicial") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tardeIntervalo" > tardeIntervalo</Label>

<Input id="tardeIntervalo" type = "number" step = "any" {...register("tardeIntervalo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tardeQtde" > tardeQtde</Label>

<Input id="tardeQtde" type = "number" step = "any" {...register("tardeQtde") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dom" > dom</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="dom"
checked = { watch("dom") }
onCheckedChange = {(checked) => setValue("dom", !!checked)}
              />
  < label htmlFor = "dom" className = "text-sm font-normal" >
    { watch("dom") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="seg" > seg</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="seg"
checked = { watch("seg") }
onCheckedChange = {(checked) => setValue("seg", !!checked)}
              />
  < label htmlFor = "seg" className = "text-sm font-normal" >
    { watch("seg") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="ter" > ter</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ter"
checked = { watch("ter") }
onCheckedChange = {(checked) => setValue("ter", !!checked)}
              />
  < label htmlFor = "ter" className = "text-sm font-normal" >
    { watch("ter") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="qua" > qua</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="qua"
checked = { watch("qua") }
onCheckedChange = {(checked) => setValue("qua", !!checked)}
              />
  < label htmlFor = "qua" className = "text-sm font-normal" >
    { watch("qua") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="qui" > qui</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="qui"
checked = { watch("qui") }
onCheckedChange = {(checked) => setValue("qui", !!checked)}
              />
  < label htmlFor = "qui" className = "text-sm font-normal" >
    { watch("qui") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="sex" > sex</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="sex"
checked = { watch("sex") }
onCheckedChange = {(checked) => setValue("sex", !!checked)}
              />
  < label htmlFor = "sex" className = "text-sm font-normal" >
    { watch("sex") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="sab" > sab</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="sab"
checked = { watch("sab") }
onCheckedChange = {(checked) => setValue("sab", !!checked)}
              />
  < label htmlFor = "sab" className = "text-sm font-normal" >
    { watch("sab") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
