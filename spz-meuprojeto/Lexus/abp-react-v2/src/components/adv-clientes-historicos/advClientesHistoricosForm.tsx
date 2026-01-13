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
  import { useCreateAdvClientesHistoricos, useUpdateAdvClientesHistoricos } from "@/lib/abp/hooks/useAdvClientesHistoricoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idHistorico: z.any(),
  
    idCliente: z.any(),
  
    idProcesso: z.any(),
  
    idUsuario: z.any(),
  
    idTipoHistorico: z.any(),
  
    data: z.any(),
  
    hora: z.any(),
  
    ocorrencia: z.any(),
  
    tsInclusao: z.any(),
  
    idOportunidade: z.any(),
  
    depto: z.any(),
  
    prioritario: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvClientesHistoricosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvClientesHistoricosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvClientesHistoricosFormProps) {
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

  const createMutation = useCreateAdvClientesHistoricos();
const updateMutation = useUpdateAdvClientesHistoricos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advClientesHistoricos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advClientesHistoricos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclienteshistoricos:", error);
    toast.error(error.message || "Failed to save advclienteshistoricos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advClientesHistoricos": "Create advClientesHistoricos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclienteshistoricos." : "Fill in the details to create a new advclienteshistoricos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idHistorico" > idHistorico</Label>

<Input id="idHistorico" type = "number" step = "any" {...register("idHistorico") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario * </Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipoHistorico" > idTipoHistorico * </Label>

<Input id="idTipoHistorico" type = "number" step = "any" {...register("idTipoHistorico") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="data" > data * </Label>

<Input id="data" {...register("data") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="hora" > hora</Label>

<Input id="hora" {...register("hora") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ocorrencia" > ocorrencia</Label>

<Input id="ocorrencia" {...register("ocorrencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idOportunidade" > idOportunidade</Label>

<Input id="idOportunidade" type = "number" step = "any" {...register("idOportunidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="depto" > depto</Label>

<Input id="depto" {...register("depto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="prioritario" > prioritario</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="prioritario"
checked = { watch("prioritario") }
onCheckedChange = {(checked) => setValue("prioritario", !!checked)}
              />
  < label htmlFor = "prioritario" className = "text-sm font-normal" >
    { watch("prioritario") ?"Enabled": "Disabled" }
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
