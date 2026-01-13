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
  import { useCreateAdvCliLog, useUpdateAdvCliLog } from "@/lib/abp/hooks/useAdvCliLogs";
import { toast } from "sonner";

const formSchema = z.object({
  
    idLog: z.any(),
  
    idCliente: z.any(),
  
    idUsuario: z.any(),
  
    acao: z.any(),
  
    idArea: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idResponsavel: z.any(),
  
    dataAgendamento: z.any(),
  
    inssIdTipoBeneficio: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvCliLogFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvCliLogForm({
  isOpen,
  onClose,
  initialValues,
}: AdvCliLogFormProps) {
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

  const createMutation = useCreateAdvCliLog();
const updateMutation = useUpdateAdvCliLog();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advCliLog updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advCliLog created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclilog:", error);
    toast.error(error.message || "Failed to save advclilog");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advCliLog": "Create advCliLog" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclilog." : "Fill in the details to create a new advclilog." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idLog" > idLog</Label>

<Input id="idLog" type = "number" step = "any" {...register("idLog") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario * </Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="acao" > acao * </Label>

<Input id="acao" type = "number" step = "any" {...register("acao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idArea" > idArea * </Label>

<Input id="idArea" type = "number" step = "any" {...register("idArea") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idResponsavel" > idResponsavel</Label>

<Input id="idResponsavel" type = "number" step = "any" {...register("idResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataAgendamento" > dataAgendamento</Label>

<Input id="dataAgendamento" type = "date" {...register("dataAgendamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssIdTipoBeneficio" > inssIdTipoBeneficio</Label>

<Input id="inssIdTipoBeneficio" type = "number" step = "any" {...register("inssIdTipoBeneficio") } />

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
