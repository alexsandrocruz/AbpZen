import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreCheckListsGruposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreCheckListsGruposes(input: GetAdvPreCheckListsGruposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreCheckListsGruposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-check-lists-grupos", {
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

export function useAllAdvPreCheckListsGruposes() {
  return useQuery({
    queryKey: ["advPreCheckListsGruposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-check-lists-grupos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreCheckListsGrupos(id: string) {
  return useQuery({
    queryKey: ["advPreCheckListsGrupos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-check-lists-grupos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreCheckListsGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-check-lists-grupos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreCheckListsGruposes"] });
    },
  });
}

export function useUpdateAdvPreCheckListsGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-check-lists-grupos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreCheckListsGruposes"] });
      queryClient.invalidateQueries({ queryKey: ["advPreCheckListsGrupos", data.id] });
    },
  });
}

export function useDeleteAdvPreCheckListsGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-check-lists-grupos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreCheckListsGruposes"] });
    },
  });
}


