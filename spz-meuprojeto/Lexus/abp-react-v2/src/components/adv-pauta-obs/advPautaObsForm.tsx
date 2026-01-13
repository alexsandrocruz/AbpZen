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
  import { useCreateAdvPautaObs, useUpdateAdvPautaObs } from "@/lib/abp/hooks/useAdvPautaObses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idPautaObs: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    id: z.any(),
  
    idTipo: z.any(),
  
    observacao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvPautaObsFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvPautaObsForm({
  isOpen,
  onClose,
  initialValues,
}: AdvPautaObsFormProps) {
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

  const createMutation = useCreateAdvPautaObs();
const updateMutation = useUpdateAdvPautaObs();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advPautaObs updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advPautaObs created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advpautaobs:", error);
    toast.error(error.message || "Failed to save advpautaobs");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advPautaObs": "Create advPautaObs" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advpautaobs." : "Fill in the details to create a new advpautaobs." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idPautaObs" > idPautaObs</Label>

<Input id="idPautaObs" type = "number" step = "any" {...register("idPautaObs") } />

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
  <Label htmlFor="id" > id</Label>

<Input id="id" type = "number" step = "any" {...register("id") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipo" > idTipo</Label>

<Input id="idTipo" {...register("idTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="observacao" > observacao</Label>

<Input id="observacao" {...register("observacao") } />

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
