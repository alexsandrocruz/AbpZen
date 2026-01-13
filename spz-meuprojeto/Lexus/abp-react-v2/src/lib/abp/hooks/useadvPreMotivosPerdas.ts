import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreMotivosPerdasInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreMotivosPerdas(input: GetAdvPreMotivosPerdasInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreMotivosPerdas", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-motivos-perda", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAdvPreMotivosPerdas() {
  return useQuery({
    queryKey: ["advPreMotivosPerdas", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-motivos-perda", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreMotivosPerda(id: string) {
  return useQuery({
    queryKey: ["advPreMotivosPerda", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-motivos-perda/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreMotivosPerda() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-motivos-perda", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreMotivosPerdas"] });
    },
  });
}

export function useUpdateAdvPreMotivosPerda() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-motivos-perda/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreMotivosPerdas"] });
      queryClient.invalidateQueries({ queryKey: ["advPreMotivosPerda", data.id] });
    },
  });
}

export function useDeleteAdvPreMotivosPerda() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-motivos-perda/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreMotivosPerdas"] });
    },
  });
}


